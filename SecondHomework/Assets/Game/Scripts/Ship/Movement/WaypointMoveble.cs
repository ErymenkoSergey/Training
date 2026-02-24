using System;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    [Serializable]
    public sealed class WaypointMoveble
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private float _stoppingDistance = 0.25f;
        
        public (Vector3, bool) MoveShipToWaypoint(Vector2 destination)
        {
            Vector2 distance = destination - (Vector2)this.rigidbody.transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            Vector2 moveDirection = isNotReached ? distance.normalized : Vector3.zero;
            
            return (moveDirection, isNotReached);
        }
    }
}