using System;
using Game.Data;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Configuration;
using Modules.Utils;
using Unity.Collections;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public class BaseShip : MonoBehaviour, IHealth, IMovable, IShot
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDead;
        
        [Header("Data")]
        [SerializeField] private ShipData config;

        private int currentHealth;
        private int maxHealth => config.Health;
        private const int DEAD_VALUE_HEALTH = 0;
        
        [Header("Combat")] 
        [SerializeField] private Transform firePoint;
        public Transform FirePoint => firePoint;
        public float fireTime = 0f;
        
        [Header("Movement")] [SerializeField] private Engine engine;
        [Header("Visual")] [SerializeField] private VisualConfiguration visual;
        [Header("Sound")] [SerializeField] private SoundConfiguration sound;
        
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
            visual.VisualStart();
        }

        private void LateUpdate()
        {
            if (currentHealth <= DEAD_VALUE_HEALTH)
                return;
            visual.AnimateMovement(Time.deltaTime, moveDirection); // изуал - в отдельную часть корабля!!
        }

        public void ResetData() => currentHealth = config.Health;

        public void ChangeDirection(Vector2 direction)
        {
            if (currentHealth <= DEAD_VALUE_HEALTH)
                return;
            moveDirection = direction;
            engine.MoveStep(moveDirection);
            engine.FixedUpdate();
        }

        public void Fire(Vector3 direction)
        {
            float time = Time.time;
            if (time - fireTime < config.FireCooldown || currentHealth <= DEAD_VALUE_HEALTH)
                return;

            ShowEffectFire();
            iBulletSpawner.Spawn(GetBulletConfiguration(direction));
            fireTime = time;
        }

        private void ShowEffectFire()
        {
            sound.PlayFireSFX();
            visual.ShowFireVFX();
        }

        public void SetDamage(int damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, DEAD_VALUE_HEALTH, maxHealth);

            if (currentHealth <= DEAD_VALUE_HEALTH)
                Dead();
            else
                Damage();
        }

        private void Dead()
        {
            visual.AnimateDead();
            sound.PlayDeadSFX();
            OnDead?.Invoke();
        }

        private void Damage()
        {
            visual.AnimateDamage();
            sound.PlayDamageSFX();
            OnHealthChanged?.Invoke(currentHealth);
        }

        private BulletNavigation GetBulletConfiguration(Vector3 direction)
        {
            BulletNavigation bulletConfiguration = new BulletNavigation();
            bulletConfiguration.Team = config.Team;
            bulletConfiguration.Position = firePoint.position;
            bulletConfiguration.Direction = direction;
            return bulletConfiguration;
        }
    }
}