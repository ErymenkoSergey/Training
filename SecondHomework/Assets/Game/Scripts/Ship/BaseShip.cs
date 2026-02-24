using System;
using Game.Data;
using Game.Enums;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Configuration;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public abstract class BaseShip : MonoBehaviour, IHealth, IMovable, IShot
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        private IShootable iShootable;
        private IGameOver gameOver;
        protected bool isGameOver;

        [Header("Data")] [SerializeField] private ShipData config;

        [Header("Health")]
        [field: SerializeField]
        public int CurrentHealth { get; set; }

        public int DeadValueHealth { get; private set; } = 0;

        public int CurrentMaxHealth => config.Health;
        private Vector3 moveDirection;

        [Header("Combat")] [SerializeField] private Transform firePoint;
        protected Transform FirePoint => firePoint;
        protected float fireTime = 0f;

        [Header("Movement")] [SerializeField] private Engine engine;
        [Header("Visual")] [SerializeField] private VisualConfiguration visual;
        [Header("Sound")] [SerializeField] private SoundConfiguration sound;

        public void Construct(IShootable iShootable, IGameOver gameOver)
        {
            this.iShootable = iShootable;
            this.gameOver = gameOver;
            StartShip();
        }

        private void StartShip()
        {
            ResetData();
            visual.VisualStart();
        }

        private void OnDisable()
        {
            gameOver.OnGameOver -= SetGameOver;
        }

        private void LateUpdate()
        {
            if (CurrentHealth <= DeadValueHealth || isGameOver)
                return;
            visual.AnimateMovement(Time.deltaTime, moveDirection);
        }

        public void ResetData()
        {
            CurrentHealth = config.Health;
            gameOver.OnGameOver += SetGameOver;
        }

        private void SetGameOver(bool isOver) => isGameOver = isOver;

        public void ChangeDirection(Vector2 direction)
        {
            if (CurrentHealth <= DeadValueHealth || isGameOver)
                return;
            moveDirection = direction;
            engine.MoveStep(moveDirection);
            engine.FixedUpdate();
        }

        public void Fire(Vector3 direction)
        {
            float time = Time.time;
            if (time - fireTime < config.FireCooldown || CurrentHealth <= DeadValueHealth || isGameOver)
                return;

            ShowEffectFire();
            iShootable.Shoot(GetBulletConfiguration(config.Team, direction));
            fireTime = time;
        }

        private void ShowEffectFire()
        {
            sound.PlayFireSFX();
            visual.ShowFireVFX();
        }

        public void SetDamage(int health)
        {
            if (isGameOver)
                return;

            visual.AnimateDamage();
            sound.PlayDamageSFX();
            OnHealthChanged?.Invoke(health);
        }

        public void NotifyAboutDead() => OnDead?.Invoke();

        private BulletConfiguration GetBulletConfiguration(TeamType type, Vector3 direction)
        {
            BulletConfiguration bulletConfiguration = new BulletConfiguration();
            bulletConfiguration.Position = firePoint.position;
            bulletConfiguration.Direction = direction;
            bulletConfiguration.Speed = config.BulletSpeed;
            bulletConfiguration.Damage = config.BulletDamage;
            bulletConfiguration.Team = type;
            return bulletConfiguration;
        }
    }
}