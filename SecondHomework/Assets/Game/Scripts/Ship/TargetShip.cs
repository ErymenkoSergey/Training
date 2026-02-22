using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    // нарушение срп тк подразбить тк тут рабоата с ui и камерой  - слишком много ответственности , отключить от базового корабля
    public sealed class TargetShip : BaseShip, ITarget, IShot
    {
        [SerializeField] private TransformBounds _playerArea;

        public void Construct(IShootable iShootable, IGameOver gameOver)
        {
            base.iShootable = iShootable;
            base.gameOver = gameOver;
            base.StartShip(true);
        }
        
        public void Shot() => Fire(firePoint.up);

        protected override void LateUpdate()
        {
            if (isGameOver)
                return;
            
            base.LateUpdate();
            transform.position = _playerArea.ClampInBounds(transform.position);
        }
    }
}