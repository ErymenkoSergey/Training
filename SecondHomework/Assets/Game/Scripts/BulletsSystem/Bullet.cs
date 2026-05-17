using System;
using Game.Data.VFX;
using Game.Enums;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet> OnDestroy;

        private Vector2 direction;
        private int damage;
        private float speed;
        public TeamType Team { get; private set; } = TeamType.None;

        private TransformBounds levelBounds;
        private GameObject currentFlicker;
        private VFXConfiguration ivfx;

        public void Construct(TeamType team, TransformBounds levelBounds, VFXConfiguration ivfx,
            BulletConfiguration config)
        {
            Team = team;
            this.levelBounds = levelBounds;
            this.ivfx = ivfx;
            speed = config.Speed;
            damage = config.Damage;
            gameObject.layer = LayerMask.NameToLayer(config.BulletNameMask);
        }

        public void SetArgs(BulletArgs args)
        {
            direction = args.Direction;
            transform.position = args.Position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);

            FlickerEffect(true);
        }

        private void FlickerEffect(bool isCreate)
        {
            if (ivfx == null)
                return;

            if (isCreate)
            {
                var vfxPrefab = ivfx.GetFlickerVFX(Team);
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