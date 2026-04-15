using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Inputs;
using Game.Mechanics.Ship;
using Game.Mechanics.Spawner;
using UnityEngine;

namespace Game.Core
{
    public sealed class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;
        [SerializeField] private PlayerInput input;
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private BulletManager bulletManager;
        [SerializeField] private PlayerShip player;
        private BaseShip playerShip;
        private ITarget target => player;
        private IMovable iMovable;
        private IShot iShot;
        private IShipStatus iShipStatus;
        private IBulletSpawner IBulletSpawner => bulletManager;

        private void Awake() => SetRef();

        private void SetRef()
        {
            playerShip = player.Ship;
            iMovable = playerShip;
            iShot = playerShip;
            iShipStatus = playerShip;

            gameLoop.SetPlayerStatus(iShipStatus);
            playerShip.Construct(IBulletSpawner);
            input.Construct(iMovable, iShot, gameLoop);
            enemyController.Construct(IBulletSpawner, target, gameLoop);
        }
    }
}