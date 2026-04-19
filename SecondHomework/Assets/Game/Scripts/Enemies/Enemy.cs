using Game.Interfaces;
using Game.Mechanics.Config;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private BaseShip ship;
        [SerializeField] private WaypointMoveble waypointMoveble;
        [SerializeField] private ShootingOnCooldown cooldown;

        private IPool<Enemy> respawn;
        private Vector2 destination;
        private ITarget target;
        private bool isFinished;

        public void SetData(EnemyConfiguration config)
        {
            if (ship == null)
                Debug.LogError($"Enemy Ship ref is null");

            target = config.Target;
            transform.position = config.SpawnPosition;
            destination = config.AttackPosition;
            respawn = config.Respawn;
            cooldown.SetData(target.GetTransform(), ship.IShot);
            ship.Construct(config.BulletSpawner);
            ship.OnDead += OnCharacterDead;
            target.OnDestroyed += TargetDestroyed;
        }

        private void TargetDestroyed()
        {
            isFinished = true;
        }

        private void OnDisable()
        {
            ship.OnDead -= OnCharacterDead;
            target.OnDestroyed -= TargetDestroyed;
        }

        private void FixedUpdate()
        {
            if (ship == null || isFinished)
                return;

            var info = waypointMoveble.MoveShipToWaypoint(destination);
            if (info.Item2)
                ship.ChangeDirection(info.Item1.normalized);
            else
                cooldown.ShootingCooldown();
        }

        public void ResetData() => ship.ResetData();
        private void OnCharacterDead() => respawn.Return(this);
    }
}