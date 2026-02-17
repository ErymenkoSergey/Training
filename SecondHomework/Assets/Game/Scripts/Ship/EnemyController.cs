using System.Collections;
using System.Collections.Generic;
using Game.Enums;
using Game.Interface;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Config;
using Game.Mechanics.Ship;
using Modules.UI;
using Modules.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// func: spawn enemy and shoot to cooldown.
namespace Game.Mechanics.Spawner
{
    public sealed class EnemyController : MonoBehaviour, IEnemyRespawn
    {
        [Header("Spawn Settings")]
        [SerializeField]
        private float minSpawnCooldown = 2;

        [SerializeField]
        private float maxSpawnCooldown = 3;
        
        private float spawnCooldown;
        private float spawnTime;
        
        [FormerlySerializedAs("_prefab")]
        [Header("Pool")]
        [SerializeField]
        private Enemy prefab;

        [SerializeField]
        private Transform _container;
        
        private readonly Queue<Enemy> pool = new();

        [Header("Target")]
        [SerializeField]
        private PlayerShip playerShip;
        private IPlayer target;
        private IHealth healthPlayer;
        
        [Header("Points")]
        [SerializeField]
        private Transform[] _spawnPositions;
        
        [SerializeField]
        private Transform[] _attackPositions;
        
        private int spawnIndex;
        private int attackIndex;
        
        [Header("UI")]
        [SerializeField]
        private ScoreView scoreView;
        
        private int destroyedEnemies;
        
        private void Awake()
        {
            TargetConfiguration();
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
            scoreView.SetValue(destroyedEnemies);
            ResetSpawnCooldown();
        }
        
        private EnemyConfiguration GetConfiguration()
        {
            EnemyConfiguration config = new EnemyConfiguration();
            config.SpawnPosition = NextSpawnPosition();
            config.AttackPosition = NextDestination();
            config.Target = target;
            config.TargetHealth = healthPlayer;
            config.Respawn = this;
            return config;
        }

        private void FixedUpdate()
        {
            float time = Time.fixedTime;
            if (time - spawnTime < spawnCooldown || healthPlayer.CurrentHealth <= healthPlayer.DeadValueHealth)
                return;
            
            if (pool.TryDequeue(out Enemy enemy))
                enemy.gameObject.SetActive(true);
            else
                enemy = Instantiate(prefab, _container);

            enemy.SetData(GetConfiguration());
                
            ResetSpawnCooldown();
        }

        private void TargetConfiguration()
        {
            if (playerShip == null)
                Debug.LogError($"{nameof(playerShip)} is null!!!");

            target = playerShip;
            healthPlayer = playerShip;
        }
        
        private void ResetSpawnCooldown()
        {
            spawnCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
            spawnTime = Time.fixedTime;
        }

        public void Respawn(Enemy enemy)
        {
            // enemy.OnFire -= this.OnFire;
            destroyedEnemies++;
            scoreView.SetValue(destroyedEnemies);
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            enemy.ResetData();
            pool.Enqueue(enemy);
        }
        
        // private void OnFire(BaseShip enemy)
        // {
        //     Vector2 position = enemy.transform.position;
        //     Vector2 target = _player.transform.position;
        //     Vector2 direction = (target - position).normalized;
        //     // enemy.Gunner.Shoot(enemy.GetBulletConfiguration(TeamType.Enemy, direction));
        // }
        
        
        
        // private BulletConfiguration GetBulletConfiguration(BaseShip enemy)
        // {
        //     Vector2 position = enemy.firePoint.position;
        //     Vector2 target = _player.transform.position;
        //     Vector2 direction = (target - position).normalized;
        //     
        //     BulletConfiguration bulletConfiguration = new BulletConfiguration();
        //     bulletConfiguration.Position = position;
        //     bulletConfiguration.Direction = direction;
        //     bulletConfiguration.Speed = enemy.bulletSpeed;
        //     bulletConfiguration.Damage = enemy.bulletDamage;
        //     bulletConfiguration.Team = TeamType.Enemy;
        //     return bulletConfiguration;
        // }
        
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