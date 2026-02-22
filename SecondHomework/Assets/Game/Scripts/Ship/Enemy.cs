using Game.Interfaces;
using Game.Mechanics.Config;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : BaseShip // неправильный дизай, тут должно быть делегирование (лисков)  - не должен вызывать базовае методы?
    {
        [SerializeField] private float _fireCooldown = 1.25f; // fire config 
        // [SerializeField] private float _stoppingDistance = 0.25f;
        // private Vector2 destination;
        private ITarget targetTransform;
        private IEnemyRespawn respawn;

        public void SetData(EnemyConfiguration config)
        {
            transform.position = config.SpawnPosition;
            // destination = config.AttackPosition;
            targetTransform = config.Target;
            respawn = config.Respawn;
            iShootable = config.Shootable;
            gameOver = config.GameOver;
            base.StartShip(false, config.AttackPosition);
        }

        private void OnEnable() => OnDead += OnCharacterDead;

        private void OnDisable() => OnDead -= OnCharacterDead;

        protected override void FixedUpdate() // убрать логику движения и стрельбы в движёк!!!
        {
            // base.FixedUpdate();
            // Vector2 distance = destination - (Vector2)this.transform.position; // enemy
            // bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            //
            // moveDirection = isNotReached ? distance.normalized : Vector3.zero;

            // if (isNotReached)
            // {
            //     ChangeDirection(distance.normalized);
            // }
            // else
            
            // получить сигнал на огонь. 
            {
                float time = Time.time;
                if (time - FireTime >= _fireCooldown)
                {
                    Fire(GetTarget()); // Убрать в класс по стрельбе!!!
                    FireTime = time;
                }
            }
        }

        private void OnCharacterDead() => respawn.Respawn(this);

        private Vector3 GetTarget()
        {
            Vector2 position = firePoint.position;
            Vector2 target = targetTransform.transform.position;
            Vector2 direction = (target - position).normalized;
            return direction;
        }
    }
}