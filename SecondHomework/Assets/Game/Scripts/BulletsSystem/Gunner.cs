using System.Collections.Generic;
using Game.Data;
using Game.Enums;
using Game.Interface;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem.Data
{
    [CreateAssetMenu(menuName = "Game/Gunner", order = 2)]
    public sealed class Gunner : ScriptableObject
    {
        [SerializeField] private BulletUnit _prefab;
        [SerializeField] private ExplosionVFXEffectData _configView; 
        private BulletField bulletField;

        private readonly Stack<BulletUnit> _pool = new();
        [SerializeField, Range(1, 100)] private int startSizePool = 15;
        private readonly List<BulletUnit> _bullets = new();
        
        public void Init(BulletField bulletField)
        {
            this.bulletField = bulletField;
            
            if (_prefab == null || bulletField == null)
            {
                Debug.LogError("null");
                return;
            }
            
            for (var i = 0; i < startSizePool; i++)
            {
                BulletUnit bullet = Instantiate(_prefab, bulletField.Container);
                bullet.gameObject.SetActive(false);
                _pool.Push(bullet);
            }
        }

        public void FixedUpdate()
        {
            if (_bullets.Count == 0 && bulletField != null)
                return;
            
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                BulletUnit bullet = _bullets[i];
                Vector3 moveStep = bullet.direction * bullet.speed * Time.fixedDeltaTime;
                bullet.transform.position += moveStep;

                if (!bulletField.LevelBounds.InBounds(bullet.transform.position))
                {
                    _bullets.RemoveAt(i);

                    bullet.OnTriggerEntered -= this.OnTriggerEntered;
                    bullet.gameObject.SetActive(false);
                    _pool.Push(bullet);
                }
            }
        }

        public void Shoot(BulletConfiguration config)
        {
            if (config.Team == TeamType.None)
            {
                Debug.LogError($"Spawn bullet => team: {config.Team}");
                return;
            }
            
            if (_pool.TryPop(out BulletUnit bullet))
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(_prefab, bulletField.Container);

            bullet.SetData(config);
            bullet.OnTriggerEntered += this.OnTriggerEntered;
            _bullets.Add(bullet);
        }

        private void OnTriggerEntered(BulletUnit bullet, Collider2D other) 
        {
            if (!other.TryGetComponent(out IHealth ship))
                return;
            
            bool isDead = false;
            
            if (bullet.damage > 0)
            {
                ship.CurrentHealth = Mathf.Clamp(ship.CurrentHealth - bullet.damage, ship.DeadValueHealth, ship.CurrentMaxHealth);
                ship.NotifyAboutHealthChanged(ship.CurrentHealth);

                if (ship.CurrentHealth <= ship.DeadValueHealth)
                {
                    ship.NotifyAboutDead();
                    isDead = true;
                }
            }

            bullet.OnTriggerEntered -= this.OnTriggerEntered;

            _bullets.Remove(bullet);

            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);

            SpawnVFX(bullet.transform, bullet.GetTeam(), isDead);
        }

        private void SpawnVFX(Transform point, TeamType team, bool isDead = false)
        {
            if (isDead)
            {
                GameObject prefab = team == TeamType.Player ? _configView.BigExplosionVFX : _configView.ExplosionVFX;
                Instantiate(prefab, point.position, prefab.transform.rotation);
            }
            else
            {
                GameObject prefab = team == TeamType.Player ? _configView.BlueVFX : _configView.RedVFX;
                Instantiate(prefab, point.position, prefab.transform.rotation);
            }
        }
    }
}