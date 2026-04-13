using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class PlayerShip : MonoBehaviour 
    {
        [SerializeField] private BaseShip ship; 
        [SerializeField] private TransformBounds bounds;
        
        private void LateUpdate()
        {
            transform.position = bounds.ClampInBounds(transform.position);
        }
    }
}

