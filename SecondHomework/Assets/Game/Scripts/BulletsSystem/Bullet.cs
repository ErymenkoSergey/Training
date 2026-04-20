using System;
using Game.Data.VFX;
using Game.Enums;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem.Data;
using Modules.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet> OnDestroy;
        
        private Vector2 direction;
        private int damage;
        private float speed;
        private TeamType team = TeamType.None;

        [FormerlySerializedAs("config")] [SerializeField] private BulletSystemConfiguration configuration;
        [SerializeField] private TransformBounds levelBounds;
        private GameObject currentFlicker;

        private Ivfx ivfx;

        public void Construct(TransformBounds levelBounds) => this.levelBounds = levelBounds; // сделать монобеховскую фабрику

        public void SetArgs(Ivfx ivfx, BulletArgs args)
        {
            this.ivfx = ivfx;

            team = args.Team;
            direction = args.Direction;
            transform.position = args.Position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);

            FlickerEffect(true);
            ConfigurationBullet();
        }

        private void ConfigurationBullet()
        {
            if (team != TeamType.None)
            {
                var data = configuration.GetBulletConfiguration(team);
                speed = data.Speed;
                damage = data.Damage;
                gameObject.layer = LayerMask.NameToLayer(data.BulletNameMask);
            }
        }

        private void FlickerEffect(bool isCreate)
        {
            if (ivfx == null)
                return;
            
            if (isCreate)
            {
                var vfxPrefab = ivfx.GetFlickerVFX(team);
                currentFlicker = Instantiate(vfxPrefab, transform);
                currentFlicker.transform.SetParent(transform);
            }
            else
            {
                if (currentFlicker != null)
                    Destroy(currentFlicker);
            }
        }

        private void FixedUpdate()
        {
            Vector3 moveStep = direction * speed * Time.fixedDeltaTime;
            transform.position += moveStep;

            if (!levelBounds.InBounds(transform.position))
                ReturnBullet();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHealth ship))
                return;

            TakeDamage(ship);
            SpawnExplosion();
            ReturnBullet();
        }

        private void ReturnBullet()
        {
            FlickerEffect(false);
            OnDestroy?.Invoke(this);
        }

        private void TakeDamage(IHealth ship)
        {
            if (damage > 0)
                ship.SetDamage(damage);
        }

        private void SpawnExplosion() => ivfx.SpawnBulletExplosionVFX(transform);
    }
}