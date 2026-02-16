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
using Random = UnityEngine.Random;

namespace Game.Mechanics.Spawner
{
    public sealed class EnemySpawner : MonoBehaviour, IEnemyRespawn
    {
        [Header("Spawn Settings")]
        [SerializeField]
        private float _minSpawnCooldown = 2;

        [SerializeField]
        private float _maxSpawnCooldown = 3;
        
        private float _spawnCooldown;
        private float _spawnTime;
        
        [Header("Pool")]
        [SerializeField]
        private Enemy _prefab;

        [SerializeField]
        private Transform _container;
        
        private readonly Queue<Enemy> _pool = new();

        [Header("Target")]
        [SerializeField]
        private PlayerShip _playerShip;
        private IPlayer _player;
        private IHealth _healthPlayer;
        
        [Header("Points")]
        [SerializeField]
        private Transform[] _spawnPositions;
        
        [SerializeField]
        private Transform[] _attackPositions;
        
        private int _spawnIndex;
        private int _attackIndex;
        
        [SerializeField] private string enemyMask = "EnemyBullet";
        
        [Header("UI")]
        [SerializeField]
        private ScoreView _scoreView;
        
        private int _destroyedEnemies;
        
        private void Awake()
        {
            TargetConfiguration();
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
            _scoreView.SetValue(_destroyedEnemies);
        }
        
        private void Start()
        {
            ResetSpawnCooldown();
        }

        private EnemyConfiguration GetConfiguration()
        {
            EnemyConfiguration config = new EnemyConfiguration();
            config.SpawnPosition = NextSpawnPosition();
            config.AttackPosition = NextDestination();
            config.TargetHealth = _healthPlayer;
            config.Respawn = this;
            return config;
        }

        private void FixedUpdate()
        {
            float time = Time.fixedTime;
            if (time - _spawnTime < _spawnCooldown || _healthPlayer.CurrentHealth <= _healthPlayer.DeadValueHealth)
                return;
            
            if (_pool.TryDequeue(out Enemy enemy))
                enemy.gameObject.SetActive(true);
            else
                enemy = Instantiate(_prefab, _container);

            enemy.SetData(GetConfiguration());
            enemy.OnFire += this.OnFire;
                
            ResetSpawnCooldown();
        }

        private void TargetConfiguration()
        {
            if (_playerShip == null)
                Debug.LogError($"{nameof(_playerShip)} is null!!!");

            _player = _playerShip;
            _healthPlayer = _playerShip;
        }
        
        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }

        public void Respawn(Enemy enemy)
        {
            enemy.OnFire -= this.OnFire;
            Debug.Log($"R1 espawn enemy {enemy.gameObject.name}");
            _destroyedEnemies++;
            _scoreView.SetValue(_destroyedEnemies);
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            Debug.Log($"R2 espawn enemy {enemy.gameObject.name}");
            enemy.gameObject.SetActive(false);
            enemy.ResetData();
            _pool.Enqueue(enemy);
        }
        
        private void OnFire(BaseShip enemy)
        {
            enemy.Gunner.Shoot(GetBulletConfiguration(enemy));
        }
        
        private BulletConfiguration GetBulletConfiguration(BaseShip enemy)
        {
            Vector2 position = enemy.firePoint.position;
            Vector2 target = _player.transform.position;
            Vector2 direction = (target - position).normalized;
            
            BulletConfiguration bulletConfiguration = new BulletConfiguration();
            bulletConfiguration.Position = position;
            bulletConfiguration.Direction = direction;
            bulletConfiguration.Speed = enemy.bulletSpeed;
            bulletConfiguration.Damage = enemy.bulletDamage;
            bulletConfiguration.Team = TeamType.Enemy;
            bulletConfiguration.BulletNameMask = enemyMask;
            return bulletConfiguration;
        }
        
        private Vector3 NextSpawnPosition()
        {
            if (_spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                _spawnIndex = 0;
            }

            return _spawnPositions[_spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (_attackIndex >= _attackPositions.Length)
            {
                _attackPositions.Shuffle();
                _attackIndex = 0;
            }

            return _attackPositions[_attackIndex++].position;
        }
    }
}