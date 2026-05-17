using Game.Interfaces;
using Game.Mechanics.Args;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField] private BaseShip ship;
        [SerializeField] private WaypointMoveble waypointMoveble;
        [SerializeField] private ShootingOnCooldown cooldown;
        
        private ITarget target;
        private IPool<Enemy> respawn;
        private Vector2 destination;
        private bool isFinished;

        public void Construct(EnemyConstruct construct)
        {
            target = construct.Target;
            respawn = construct.Respawn;
            ship.Construct(construct.BulletSpawner);
        }

        public void SetArgs(EnemyArgs config)
        {
            if (ship == null)
                Debug.LogError($"Enemy Ship ref is null");

            transform.position = config.SpawnPosition;
            destination = config.AttackPosition;
            cooldown.SetData(target.GetTransform(), ship.IShot);
            ship.OnDead += OnCharacterDead;
            target.OnDestroyed += TargetDestroyed;
            ResetData();
        }
        
        private void ResetData() => ship.ResetData();
        private void OnCharacterDead() => respawn.Return(this);
        private void TargetDestroyed() => isFinished = true;
        
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
    }
}