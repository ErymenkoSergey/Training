using Game.Interfaces;
using Game.Mechanics.Config;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : MonoBehaviour // тут должно быть не наследование а дилигирование
                                         // (он должен говрить что должен делать базовый корабль)
    {
        private IEnemyRespawn respawn;
        private Vector2 destination;
        [SerializeField] private BaseShip ship;

        [SerializeField] private WaypointMoveble waypointMoveble;
        [SerializeField] private ShootingOnCooldown cooldown;

        public void SetData(EnemyConfiguration config)
        {
            transform.position = config.SpawnPosition;
            destination = config.AttackPosition;
            respawn = config.Respawn;
            cooldown.SetData(ship.FirePoint, config.Target, ship.fireTime, ship);
            ship.Construct(config.BulletSpawner);
            // ship.OnDead += OnCharacterDead;
        }

        // private void OnDisable() => OnDead -= OnCharacterDead;

        private void FixedUpdate()
        {
            // if (isGameOver)
            //     return;

            var info = waypointMoveble.MoveShipToWaypoint(destination);

            if (info.Item2)
                ship.ChangeDirection(info.Item1.normalized);
            else
                cooldown.ShootingCooldown();
        }

        public void ResetData() => ship.ResetData();

        // private void OnCharacterDead() => respawn.Respawn(this);
    }
}