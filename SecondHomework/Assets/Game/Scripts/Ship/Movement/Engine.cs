using System;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    [Serializable]
    public sealed class Engine
    {
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private float speed;

        private Vector2? direction;
        
        public void MoveStep(Vector2 direction) => this.direction = direction;

        public void FixedUpdate()
        {
            if (!this.direction.HasValue)
                return;

            Vector2 direction = this.direction.Value;
            Vector2 newPosition = rigidbody.position + direction * (speed * Time.fixedDeltaTime);
            rigidbody.MovePosition(newPosition);
            this.direction = null;
        }
    }
}