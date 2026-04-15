using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    [CreateAssetMenu(menuName = "Game/BulletConfiguration", order = 4)]
    public sealed class BulletConfiguration : ScriptableObject 
    {
        public float Speed;
        public int Damage;
        public string BulletNameMask;
    }
}