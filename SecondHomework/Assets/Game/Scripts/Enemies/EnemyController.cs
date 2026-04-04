using System.Collections;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Mechanics.Config;
using Game.Mechanics.Ship;
using Modules.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Mechanics.Spawner
{
    public sealed class EnemyController : MonoBehaviour, IEnemyRespawn
    { 
        private IBulletSpawner iBulletSpawner;
        private Transform target;
        private IScore iScore;

        #region Logic Cooldown

        [Header("Spawn Settings")]
        [SerializeField]
        private float minSpawnCooldown = 2;

        [SerializeField] 
        private float maxSpawnCooldown = 3;

        private float spawnCooldown;
        private float spawnTime;

        #endregion

        // logic spawn 
        [SerializeField]
        private Enemy prefab;

        [SerializeField] private Transform _container;

        private readonly Queue<Enemy> pool = new();

        [Header("Points")] [SerializeField] private Transform[] _spawnPositions;

        [SerializeField] private Transform[] _attackPositions;

        private int spawnIndex;
        private int attackIndex;

        private int destroyedEnemies;

        private IGameLoop iGameLoop;
        private bool isGameOver;

        public void Construct(IBulletSpawner iBulletSpawner, Transform target, IScore iScore, IGameLoop gameLoop)
        {
            this.iBulletSpawner = iBulletSpawner;
            this.target = target;
            this.iScore = iScore;
            iGameLoop = gameLoop;
            iGameLoop.OnGameOver += SetGameLoop;
            StartSystem();
        }

        private void StartSystem()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
            iScore.ChangeScore(destroyedEnemies);
            ResetSpawnCooldown();
        }
        
        private void OnDisable()
        {
            if (iGameLoop != null)
                iGameLoop.OnGameOver -= SetGameLoop;
        }

        private void SetGameLoop(bool isOver) => isGameOver = isOver;
        
        public void Respawn(Enemy enemy)
        {
            destroyedEnemies++;
            iScore.ChangeScore(destroyedEnemies);
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private void FixedUpdate()
        {
            if (isGameOver)
                return;

            float time = Time.fixedTime;
            if (time - spawnTime < spawnCooldown)
                return;

            CreateEnemy();
        }

        private void CreateEnemy()
        {
            if (pool.TryDequeue(out Enemy enemy))
                enemy.gameObject.SetActive(true);
            else
                enemy = Instantiate(prefab, _container);

            enemy.SetData(GetConfiguration());

            ResetSpawnCooldown();
        }

        private EnemyConfiguration GetConfiguration()
        {
            EnemyConfiguration config = new EnemyConfiguration();
            config.SpawnPosition = NextSpawnPosition();
            config.AttackPosition = NextDestination();
            config.Target = target;
            config.Respawn = this;
            config.BulletSpawner = iBulletSpawner;
            config.GameLoop = iGameLoop;
            return config;
        }

        private void ResetSpawnCooldown()
        {
            spawnCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
            spawnTime = Time.fixedTime;
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            enemy.ResetData();
            pool.Enqueue(enemy);
        }

        private Vector3 NextSpawnPosition()
        {
            if (spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                spawnIndex = 0;
            }

            return _spawnPositions[spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (attackIndex >= _attackPositions.Length)
            {
                _attackPositions.Shuffle();
                attackIndex = 0;
            }

            return _attackPositions[attackIndex++].position;
        }
    }
}