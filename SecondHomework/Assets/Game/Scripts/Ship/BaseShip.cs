using System;
using Game.Data;
using Game.Enums;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Configuration;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    // этот занимается и логикой системы и вьюшной логикой - подразбить 
    public abstract class BaseShip : MonoBehaviour, IHealth
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDead; // избавиться от евентов 

        protected IGameOver gameOver;
        private bool isGameOver;
        protected IShootable iShootable;

        [Header("Data")] [SerializeField] private ShipData config;

        [Header("Health")]
        [field: SerializeField]
        public int CurrentHealth { get; set; }

        public int DeadValueHealth { get; private set; } = 0; // Const?

        public int CurrentMaxHealth => config.Health;

        [Header("Combat")] public Transform firePoint; //?
        public float FireTime = 0f; //??

        [Header("Movement")] [SerializeField] protected Engine engine;

        protected Vector3 moveDirection;

        [Header("Visual")] [SerializeField] private VisualConfiguration visual;

        [Header("Sound")] [SerializeField] private SoundConfiguration sound;

        protected void StartShip()
        {
            ResetData();
            visual.VisualStart();
        }

        private void OnDisable()
        {
            gameOver.OnGameOver -= SetGameOver;
        }

        protected virtual void FixedUpdate()
        {
            if (isGameOver)
                return;

            engine?.FixedUpdate(); // Этот двигатель должен двигать корабли 
        }

        protected virtual void LateUpdate()
        {
            if (CurrentHealth <= DeadValueHealth || isGameOver)
                return;

            visual.AnimateMovement(Time.deltaTime, moveDirection);
        }

        private void SetGameOver(bool isOver) => isGameOver = isOver;

        public void ResetData()
        {
            CurrentHealth = config.Health;
            gameOver.OnGameOver += SetGameOver;
        }

        protected void Fire(Vector3 direction)
        {
            float time = Time.time;
            if (time - FireTime < config.FireCooldown || CurrentHealth <= DeadValueHealth || isGameOver)
                return;

            ShowEffectFire();
            iShootable.Shoot(GetBulletConfiguration(config.Team, direction));
            FireTime = time;
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