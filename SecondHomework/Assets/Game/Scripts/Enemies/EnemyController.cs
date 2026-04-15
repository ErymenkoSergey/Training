using System.Collections;
using System.Collections.Generic;
using Game.Interfaces;
using Game.Mechanics.Config;
using Game.Mechanics.Ship;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Mechanics.Spawner
{
    public sealed class EnemyController : MonoBehaviour, IPool<Enemy>
    { 
        [Header("Spawn Settings")]
        [SerializeField]
        private float minSpawnCooldown = 2;

        [SerializeField] 
        private float maxSpawnCooldown = 3;
        private float spawnCooldown;
        private float spawnTime;

        [SerializeField]
        private Enemy prefab;

        [SerializeField] private Transform container;
       
        [Header("Points")] [SerializeField] private Transform[] spawnPositions;
        [SerializeField] private Transform[] attackPositions;

        private int spawnIndex;
        private int attackIndex;
        private int destroyedEnemies;
        
        private readonly Queue<Enemy> pool = new();
        
        private IBulletSpawner iBulletSpawner;
        private ITarget target;
        private IScore iScore;
        private IGameLoop gameLoop;

        public void Construct(IBulletSpawner iBulletSpawner, ITarget target, IScore iScore)
        {
            this.iBulletSpawner = iBulletSpawner;
            this.target = target;
            this.iScore = iScore;
            StartSystem();
        }

        private void StartSystem()
        {
            spawnPositions.Shuffle();
            attackPositions.Shuffle();
            iScore.ChangeScore(destroyedEnemies);
            ResetSpawnCooldown();
        }

        private void FixedUpdate()
        {
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
                enemy = Instantiate(prefab, container);

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
            return config;
        }

        private void ResetSpawnCooldown()
        {
            spawnCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
            spawnTime = Time.fixedTime;
        }
        
        private Vector3 NextSpawnPosition()
        {
            if (spawnIndex >= spawnPositions.Length)
            {
                spawnPositions.Shuffle();
                spawnIndex = 0;
            }

            return spawnPositions[spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (attackIndex >= attackPositions.Length)
            {
                attackPositions.Shuffle();
                attackIndex = 0;
            }

            return attackPositions[attackIndex++].position;
        }

        public void Return(Enemy enemy)
        {
            destroyedEnemies++;
            iScore.ChangeScore(destroyedEnemies);
            StartCoroutine(DespawnInNextFrame(enemy));
        }
        
        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            enemy.ResetData();
            pool.Enqueue(enemy);
        }
    }
}