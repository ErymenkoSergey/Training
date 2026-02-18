using System;
using DG.Tweening;
using Game.Data;
using Game.Enums;
using Game.Interface;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.BulletsSystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.Ship
{
    public abstract class BaseShip : MonoBehaviour, IHealth
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        [Header("Data")]
        [SerializeField] private ShipData config;
        [SerializeField] private Gunner gunner;
        [SerializeField] private VisualConfig visualConfig;

        [Header("Health")]
        [field: SerializeField]
        public int CurrentHealth { get; set; }

        public int DeadValueHealth { get; private set; } = 0;

        public int CurrentMaxHealth => config.Health;

        [Header("Combat")] 
        public Transform firePoint; //?
        public float FireTime = 0f; //??

        [FormerlySerializedAs("_motor")] [Header("Movement")] [SerializeField]
        protected Engine engine;

        protected Vector3 moveDirection;

        [Header("Visual")] [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _fireVFX;
        [SerializeField] private AudioClip _fireSFX;
        [SerializeField] private AudioClip _damageSFX;

        private Material _material;
        private Tweener _damageAnimation;

        private void Awake()
        {
            ResetData();
            _material = new Material(visualConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        protected virtual void FixedUpdate() => engine?.FixedUpdate();

        protected virtual void LateUpdate()
        {
            if (CurrentHealth <= DeadValueHealth)
                return;

            AnimateMovement(Time.deltaTime);
        }

        public void ResetData()
        {
            CurrentHealth = config.Health;
            engine.SetSpeed(config.MoveSpeed);
        }

        protected void Fire(Vector3 direction)
        {
            float time = Time.time;
            if (time - FireTime < config.FireCooldown || CurrentHealth <= DeadValueHealth)
                return;

            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);
            
            gunner.Shoot(GetBulletConfiguration(config.Team, direction));
            
            if (_fireVFX)
                _fireVFX.Play();

            FireTime = time;
        }

        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = visualConfig.MoveRotationAngle * moveDirection.y;
            shipAngles.y = visualConfig.MoveRotationAngle / 2 * moveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = visualConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }

        public void NotifyAboutHealthChanged(int health)
        {
            if (health > 0)
                this.AnimateDamage();

            this.OnHealthChanged?.Invoke(health);
        }

        public void NotifyAboutDead()
        {
            ParticleSystem prefab = visualConfig.DestroyEffectPrefab;
            Instantiate(prefab, _viewTransform.position, prefab.transform.rotation);
            OnDead?.Invoke();
        }

        private void AnimateDamage()
        {
            if (_damageAnimation.IsActive())
                _damageAnimation.Kill();

            _damageAnimation = DOVirtual.Float(
                0f, // есть смысл выносить в поля?)
                1f,
                visualConfig.HitDuration,
                progress => _material?.SetFloat(visualConfig.HitPropertyName,
                    visualConfig.HitAnimationCurve.Evaluate(progress))
            ).SetLink(_renderer.gameObject);

            if (_damageSFX)
                _audioSource.PlayOneShot(_damageSFX);
        }

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