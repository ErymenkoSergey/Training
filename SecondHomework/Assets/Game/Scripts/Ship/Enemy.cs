using Game.Interface;
using Game.Mechanics.Config;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : BaseShip
    {
        [SerializeField] private float _fireCooldown = 1.25f;
        [SerializeField] private float _stoppingDistance = 0.25f;
        private Vector2 destination;
        private IHealth hpTarget;
        private IEnemyRespawn respawn;

        public void SetData(EnemyConfiguration config)
        {
            transform.position = config.SpawnPosition;
            destination = config.AttackPosition;
            hpTarget = config.TargetHealth;
            respawn = config.Respawn;
        }

        private void OnEnable() => OnDead += OnCharacterDead;

        private void OnDisable() => OnDead -= OnCharacterDead;

        private void OnCharacterDead() => respawn.Respawn(this);

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (CurrentHealth <= DeadValueHealth || hpTarget == null || hpTarget.CurrentHealth <= DeadValueHealth)
                return;

            Vector2 distance = destination - (Vector2)this.transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            moveDirection = isNotReached ? distance.normalized : Vector3.zero;

            if (isNotReached)
            {
                _motor.MoveStep(distance.normalized);
            }
            else
            {
                float time = Time.time;
                if (time - FireTime >= _fireCooldown)
                {
                    Fire();
                    FireTime = time;
                }
            }
        }
    }
}