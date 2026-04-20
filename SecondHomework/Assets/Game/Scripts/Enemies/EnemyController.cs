using System.Collections.Generic;
using Game.Interfaces;
using Game.Mechanics.Components;
using Game.Mechanics.Config;
using Game.Mechanics.Ship;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Mechanics.Spawner
{
    public sealed class EnemyController : MonoBehaviour, IPool<Enemy>
    {
        [Header("Spawn Settings")] [SerializeField]
        private float minSpawnCooldown = 2;

        [SerializeField] private float maxSpawnCooldown = 3;
        private float spawnCooldown;
        private float spawnTime;

        [SerializeField] private Enemy prefab;
        [SerializeField] private Transform container;

        private int destroyedEnemies;
        private readonly Queue<Enemy> pool = new();
        
        [Header("Enemy Positions")]
        [SerializeField] private EnemyPositions positions;

        private IBulletSpawner iBulletSpawner;
        private ITarget target;
        private IScore iScore;
        private IGameLoop gameLoop;

        private bool isTargetDestroyed;

        public void Construct(IBulletSpawner iBulletSpawner, ITarget target, IScore iScore)
        {
            this.iBulletSpawner = iBulletSpawner;
            this.target = target;
            this.iScore = iScore;
            StartSystem();
        }

        private void StartSystem()
        {
            positions.Construct();
            iScore.ChangeScore(destroyedEnemies);
            ResetSpawnCooldown();
            target.OnDestroyed += TargetDestroy;
        }

        private void FixedUpdate()
        {
            if (isTargetDestroyed)
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
                enemy = Instantiate(prefab, container);

            enemy.SetData(GetArgs());

            ResetSpawnCooldown();
        }

        private EnemyArgs GetArgs()
        {
            EnemyArgs config = new EnemyArgs();
            config.SpawnPosition = positions.NextSpawnPosition();
            config.AttackPosition = positions.NextDestination();
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
        
        public void Return(Enemy enemy)
        {
            destroyedEnemies++;
            iScore.ChangeScore(destroyedEnemies);
            enemy.gameObject.SetActive(false);
            enemy.ResetData();
            pool.Enqueue(enemy);
        }

        private void TargetDestroy()
        {
            isTargetDestroyed = true;
            target.OnDestroyed -= TargetDestroy;
        }
    }
}