using Game.Enums;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public class BulletConfiguration
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public int Damage;
        public TeamType Team;
        public string BulletNameMask;
        public TransformBounds Bounds;
        public IPoolable Pool;
    }
}