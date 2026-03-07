using Game.Enums;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{// легковес
    public class BulletConfiguration : ScriptableObject // Эту So подсовываем в пулю! а из настроек корабля убираем настройку скорости пули
    {
        // public Vector2 Position; // передавать отдельно!!
        // public Vector2 Direction;// передавать отдельно!!
        public float Speed;
        public int Damage;
        public TeamType Team;
        public string BulletNameMask;
        // public TransformBounds Bounds;
        // public IPoolable Pool;
    }
}