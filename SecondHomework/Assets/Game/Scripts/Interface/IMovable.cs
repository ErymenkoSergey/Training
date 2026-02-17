using UnityEngine;

namespace Game.Interface
{
    public interface IMovable
    {
        void ChangeDirection(Vector2 direction);
        void Fire();
    }
}