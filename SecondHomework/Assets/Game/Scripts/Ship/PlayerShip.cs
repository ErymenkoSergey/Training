using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    // нарушение срп тк подразбить тк тут рабоата с ui и камерой  - слишком много ответственности , отключить от базового корабля
    public sealed class PlayerShip : BaseShip // Этот должен взаимодействовать с двигателем??? Он только чекат границы.. 
    {
        [SerializeField] private TransformBounds _playerArea;
// надо подумать как сделать этот ограничитель на двигатель. видимо делам 2 вида двигателей, для ручного управления и для автоматического по вейпоинтам..
      
        protected override void LateUpdate()
        {
            if (isGameOver)
                return;
            
            base.LateUpdate();
            transform.position = _playerArea.ClampInBounds(transform.position);
        }
    }
}