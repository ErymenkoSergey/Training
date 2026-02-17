using Game.Enums;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/ShipData", order = 0)]
    public class ShipData : ScriptableObject
    {
        [Header("Core")]
        
        [SerializeField] private TeamType team = TeamType.None;
        public TeamType Team => team;
        
        [field: SerializeField]
        public int Health { get; private set; } = 5;

        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 5;

        [field: SerializeField]
        public float FireCooldown { get; private set; } = 0.25f;
        
        [SerializeField] private float bulletSpeed;
        public float BulletSpeed => bulletSpeed;
        
        [SerializeField] private int bulletDamage;
        public int BulletDamage => bulletDamage;
    }
}