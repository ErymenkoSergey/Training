using System;
using DG.Tweening;
using Game.Data;
using Game.Interface;
using Game.Mechanics.BulletsSystem.Data;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public abstract class BaseShip : MonoBehaviour, IHealth
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDead;

        public event Action<BaseShip> OnFire;

        [field: SerializeField]
        public ShipData config { get; private set; }

        [field: SerializeField]
        public Gunner Gunner;
        
        [Header("Health")]
        [field: SerializeField]
        public int CurrentHealth { get; set; }

        public int DeadValueHealth { get; private set; } = 0;
        
        public int CurrentMaxHealth
        {
            get
            {
                return config.Health;
            }
        }

        [Header("Combat")]
        public Transform firePoint;
        public float bulletSpeed;
        public int bulletDamage; 
        public float FireTime;

        [Header("Movement")]
        [SerializeField]
        protected Motor _motor;
        
        protected Vector3 moveDirection;

        [Header("Visual")]
        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private Transform _viewTransform;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private ShipControllerViewConfig _viewConfig;

        [SerializeField]
        private ParticleSystem _fireVFX;

        [SerializeField]
        private AudioClip _fireSFX;

        [SerializeField]
        private AudioClip _damageSFX;

        private Material _material;
        private Tweener _damageAnimation;
        
        private void Awake()
        {
            CurrentHealth = config.Health;
            _motor.SetSpeed(config.MoveSpeed);

            _material = new Material(_viewConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        protected virtual void FixedUpdate() => _motor?.FixedUpdate();

        protected void Fire()
        {
            float time = Time.time;
            if (time - FireTime < config.FireCooldown || this.CurrentHealth <= DeadValueHealth)
                return;

            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);

            if (_fireVFX)
                _fireVFX.Play();

            this.OnFire?.Invoke(this);
            FireTime = time;
        }
        
        protected virtual void LateUpdate()
        {
            this.AnimateMovement(Time.deltaTime);
        }

        private void AnimateMovement(float deltaTime)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _viewConfig.MoveRotationAngle * moveDirection.y;
            shipAngles.y = _viewConfig.MoveRotationAngle / 2 * moveDirection.x * -1f;
            
            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _viewConfig.MoveSpeed * deltaTime;
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
            ParticleSystem prefab = _viewConfig.DestroyEffectPrefab;
            Instantiate(prefab, _viewTransform.position, prefab.transform.rotation);
            this.OnDead?.Invoke();
            gameObject.SetActive(false);
        }

        private void AnimateDamage()
        {
            if (_damageAnimation.IsActive())
                _damageAnimation.Kill();

            _damageAnimation = DOVirtual.Float(
                0f,
                1f,
                _viewConfig.HitDuration,
                progress => _material?.SetFloat(_viewConfig.HitPropertyName,
                    _viewConfig.HitAnimationCurve.Evaluate(progress))
            ).SetLink(_renderer.gameObject);

            if (_damageSFX)
                _audioSource.PlayOneShot(_damageSFX);
        }
    }
}