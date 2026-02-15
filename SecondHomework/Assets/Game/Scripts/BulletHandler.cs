using System.Collections.Generic;
using Game.Data;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletHandler : MonoBehaviour
    {
        [SerializeField] private BulletData _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private BulletViewConfig _configView;
        [SerializeField] private TransformBounds _levelBounds; 

        private readonly Stack<BulletData> _pool = new();
        private readonly List<BulletData> _bullets = new();
        
        private void Awake()
        {
            if (_prefab == null || _container == null)
            {
                Debug.LogError("BulletHandler: prefab or container cannot be null!");
                return;
            }
            
            for (var i = 0; i < 10; i++)
            {
                BulletData bullet = Instantiate(_prefab, _container);
                bullet.gameObject.SetActive(false);
                _pool.Push(bullet);
            }
        }

        private void FixedUpdate()
        {
            if (_bullets.Count == 0)
                return;
            
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                BulletData bullet = _bullets[i];
                Vector3 moveStep = bullet.direction * bullet.speed * Time.fixedDeltaTime;
                bullet.transform.position += moveStep;

                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    _bullets.RemoveAt(i);

                    bullet.OnTriggerEntered -= this.OnTriggerEntered;
                    bullet.gameObject.SetActive(false);
                    _pool.Push(bullet);
                }
            }
        }

        public void Spawn(BulletConfiguration config)
        {
            if (config.Team == TeamType.None)
            {
                Debug.LogError($"Spawn bullet => team: {config.Team}");
                return;
            }
            
            if (_pool.TryPop(out BulletData bullet))
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(_prefab, _container);

            bullet.SetData(config);
            bullet.OnTriggerEntered += this.OnTriggerEntered;
            _bullets.Add(bullet);
        }

        private void OnTriggerEntered(BulletData bullet, Collider2D other) 
        {
            if (!other.TryGetComponent(out ShipController ship))
                return;
            
            if (bullet.damage > 0)
            {
                ship.currentHealth = Mathf.Clamp(ship.currentHealth - bullet.damage, 0, ship.config.Health);
                ship.NotifyAboutHealthChanged(ship.currentHealth);

                if (ship.currentHealth <= 0)
                {
                    ship.NotifyAboutDead();
                    ship.gameObject.SetActive(false);
                }
            }

            bullet.OnTriggerEntered -= this.OnTriggerEntered;

            _bullets.Remove(bullet);

            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);

            GameObject prefab = bullet.GetTeam() == TeamType.Player && ship is Enemy
                ? _configView.RedVFX
                : _configView.BlueVFX;
            Instantiate(prefab, bullet.transform.position, prefab.transform.rotation);
        }
    }

    public class BulletConfiguration
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public int Damage;
        public TeamType Team;
        public string BulletNameMask;
    }
}