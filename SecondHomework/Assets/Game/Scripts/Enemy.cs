using Game.Interface;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : BaseShip
    {
        [Header("Enemy")] public BaseShip target;
        public Vector2 destination;

        [SerializeField] private float _fireCooldown = 1.25f;

        [SerializeField] private float _stoppingDistance = 0.25f;

        private IEnemyRespawn _despawner;

        public void SetRespawn(IEnemyRespawn respawn) => _despawner = respawn;

        private void OnEnable() => OnDead += OnCharacterDead;

        private void OnDisable() => OnDead -= OnCharacterDead;

        private void OnCharacterDead() => _despawner.Respawn(this);

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.CurrentHealth <= 0 || this.target == null || this.target.CurrentHealth <= 0)
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
                if (time - _fireTime >= _fireCooldown)
                {
                    this.Fire();
                    _fireTime = time;
                }
            }
        }
    }
}