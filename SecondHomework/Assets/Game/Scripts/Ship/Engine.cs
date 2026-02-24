using System;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    [Serializable]
    public sealed class Engine// i movement// Контроль управления идет сюда..  // передавать его через конфиг? тк два типа двигателей есть.
    {
        // public bool isManualMode = false;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed;

        private Vector2? _direction;
        private float _stoppingDistance = 0.25f;
        // private Vector2 waypoint;

        //public void SetSpeed(float speed) => _speed = speed;

        // public void SetWaypoint(Vector2 point) => waypoint = point;
        
        public void MoveStep(Vector2 direction) => _direction = direction;

        public void FixedUpdate()
        {
            if (!_direction.HasValue)
                return;

            // if (isManualMode)
            // {
                Vector2 direction = _direction.Value;
                Vector2 newPosition = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
                _rigidbody.MovePosition(newPosition);
                _direction = null;
            // }
            // else
            // {
            //     // Debug.Log($"FixedUpdate Speed: _direction {_direction} / speed {_speed} / {_stoppingDistance}");
            //     // //AttackPosition
            //     // Vector2 distance = waypoint - (Vector2)this._rigidbody.position; // enemy
            //     // bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;
            //     // _direction = isNotReached ? distance.normalized : Vector3.zero;
            //     // if (isNotReached)
            //     //     MoveStep(distance.normalized);
            //     // else
            //     // {
            //     //     Debug.Log($"Корабль на месте, отключаем двигатель: {_rigidbody.position}");
            //     // }
            // }
        }
    }
}