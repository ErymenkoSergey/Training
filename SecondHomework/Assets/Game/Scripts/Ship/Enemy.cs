using Game.Interfaces;
using Game.Mechanics.Config;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : BaseShip 
    {
        private IEnemyRespawn respawn;
        private Vector2 destination;

        [SerializeField] private WaypointMoveble waypointMoveble;
        [SerializeField] private ShootingOnCooldown cooldown;

        public void SetData(EnemyConfiguration config)
        {
            transform.position = config.SpawnPosition;
            destination = config.AttackPosition;
            respawn = config.Respawn;
            cooldown.SetData(FirePoint, config.Target, fireTime, this);
            base.Construct(config.Shootable, config.GameOver);
            base.OnDead += OnCharacterDead;
        }

        private void OnDisable() => OnDead -= OnCharacterDead;

        private void FixedUpdate()
        {
            if (isGameOver)
                return;

            var info = waypointMoveble.MoveShipToWaypoint(destination);

            if (info.Item2)
                ChangeDirection(info.Item1.normalized);
            else
                cooldown.ShootingCooldown();
        }

        private void OnCharacterDead() => respawn.Respawn(this);
    }
}