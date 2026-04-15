using System;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class PlayerShip : MonoBehaviour, ITarget
    {
        public event Action OnDestroyed;

        [SerializeField] private BaseShip ship;
        [SerializeField] private TransformBounds bounds;

        private void OnEnable()
        {
            if (ship != null)
                ship.OnDead += ShipOnOnDead;
        }

        private void OnDisable()
        {
            if (ship != null)
                ship.OnDead -= ShipOnOnDead;
        }

        private void ShipOnOnDead() => OnDestroyed?.Invoke();

        private void LateUpdate()
        {
            transform.position = bounds.ClampInBounds(transform.position);
        }

        public BaseShip Ship => ship;
        public Transform GetTransform() => transform;
    }
}