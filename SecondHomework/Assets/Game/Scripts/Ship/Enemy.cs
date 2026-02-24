using Game.Interfaces;
using Game.Mechanics.Config;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : BaseShip // неправильный дизай, тут должно быть делегирование (лисков)  - не должен вызывать базовае методы?
    {
        private IEnemyRespawn respawn;
        private Vector2 destination;

        [SerializeField] private WaypointMoveble waypointMoveble;
        [SerializeField] private ShootingOnCooldown cooldown;

        public void SetData(EnemyConfiguration config)
        {
            transform.position = config.SpawnPosition;
            destination = config.AttackPosition;
            // targetTransform = config.Target;
            respawn = config.Respawn;
            cooldown.SetData(FirePoint, config.Target, fireTime, this);
            base.Construct(config.Shootable, config.GameOver);
            base.OnDead += OnCharacterDead;
        }

        private void OnDisable() => OnDead -= OnCharacterDead;

        protected override void FixedUpdate() // убрать логику движения и стрельбы в движёк!!!
        {
            if (isGameOver)
                return;

            base.FixedUpdate();
            var info = waypointMoveble.MoveShipToWaypoint(destination);

            if (info.Item2)
                ChangeDirection(info.Item1.normalized);
            else
                cooldown.ShootingCooldown();
        }

        private void OnCharacterDead() => respawn.Respawn(this);
    }
}