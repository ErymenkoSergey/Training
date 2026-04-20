using System;
using Game.Data;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Components;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public class BaseShip : MonoBehaviour, IMovable, IHealth, IShipStatus
    {
        public event Action<int, int> OnHealthChanged;
        public event Action OnDead;
        
        [Header("Data")] [SerializeField] private ShipConfiguration config;
        [Header("Engine")] [SerializeField] private EngineComponent engineComponent;
        [Header("Health")] [SerializeField] private HealthComponent health;
        [Header("Visual")] [SerializeField] private VisualComponent visual;
        [Header("Sound")] [SerializeField] private SoundComponent sound;
        [Header("Weapon")] [SerializeField] private WeaponComponent weapon;
        public IShot IShot => weapon;

        private Vector3 moveDirection;
        private IBulletSpawner iBulletSpawner;

        public void Construct(IBulletSpawner iBulletSpawner)
        {
            this.iBulletSpawner = iBulletSpawner;
            StartShip();
        }

        private void StartShip()
        {
            ResetData();
            IShot.OnShot += FireEffect;
            visual.VisualStart();
        }

        private void LateUpdate()
        {
            if (health.IsDead)
                return;

            visual.AnimateMovement(Time.deltaTime, moveDirection);
        }

        public void ResetData()
        {
            health.SetHealth(config.Health);
            health.SetHealthMax(config.Health);
        }

        public void ChangeDirection(Vector2 direction)
        {
            if (health.IsDead)
                return;

            moveDirection = direction;
            engineComponent.MoveStep(moveDirection);
            engineComponent.FixedUpdate();
        }

        private void FireEffect(Vector3 direction)
        {
            if (health.IsDead)
                return;
            
            ShowEffectFire();
            iBulletSpawner.Spawn(GetBulletConfiguration(direction));
        }

        private void ShowEffectFire()
        {
            sound.PlayFireSFX();
            visual.ShowFireVFX();
        }

        public void SetDamage(int damage)
        {
            health.TakeDamage(damage);
            OnHealthChanged?.Invoke(health.CurrentHealth, health.CurrentMaxHealth);
            visual.AnimateDamage();

            if (health.IsDead)
                Dead();
            else
                sound.PlayDamageSFX();
        }

        private void Dead()
        {
            sound.PlayDeadSFX();
            OnDead?.Invoke();
            IShot.OnShot -= FireEffect;
            config.VFXConfiguration.SpawnShipExplosionVFX(transform);
        }
        
        public GameObject GetShip() => gameObject;

        private BulletArgs GetBulletConfiguration(Vector3 direction)
        {
            BulletArgs bulletConfiguration = new BulletArgs();
            bulletConfiguration.Team = config.Team;
            bulletConfiguration.Position = weapon.FirePoint.position;
            bulletConfiguration.Direction = direction;
            return bulletConfiguration;
        }
    }
}