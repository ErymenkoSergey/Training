using System;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Inputs;
using Game.Mechanics.Ship;
using Game.Mechanics.Spawner;
using UnityEngine;

namespace Game.Core
{
    // при паузе - можно в пуле врагов - сменить флаг на ытоп игра - перебором
    // нужен момобех на сцене для ссылок на игрока - систему ui / и спавнер врагов, + в нем и будет ивент на стоп игры...
    public sealed class Gameplay : MonoBehaviour, IGameOver
    {
        public event Action<bool> OnGameOver;
        
        [SerializeField] private TargetShip playerShip;
        private ITarget iTarget => playerShip;
        private IMovable iMovable => playerShip;
        private IShot iShot => playerShip;

        [SerializeField] private PlayerInput input;
        [SerializeField] private UIHandler uiHandler;
        private IViewHealth iViewHealth => uiHandler;
        private IScore iScore => uiHandler;

        [SerializeField] private EnemyController enemyController;

        [SerializeField] private BulletManager bulletManager;
        private IShootable iShootable => bulletManager;
        
       
        
        private void Awake()
        {
            SetRef();
        }

        private void SetRef()
        {
            playerShip.Construct(iShootable, iViewHealth, this);
            input.Construct(iMovable, iShot, this);
            enemyController.Construct(iShootable, iTarget, iScore, this);
        }

        public void CallGameOver()
        {
            OnGameOver?.Invoke(true);
            uiHandler.GameOver();
        }
    }
}