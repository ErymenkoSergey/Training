using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class PlayerShip : BaseShip 
    {
        [SerializeField] private TransformBounds _playerArea;
        
        private void LateUpdate()
        {
            if (isGameOver)
                return;
            
            transform.position = _playerArea.ClampInBounds(transform.position);
        }
    }
}