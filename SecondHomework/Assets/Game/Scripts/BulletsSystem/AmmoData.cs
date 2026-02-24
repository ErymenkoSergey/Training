using UnityEngine;

namespace Game.Mechanics.BulletsSystem.Data
{
    [CreateAssetMenu(menuName = "Game/AmmoData", order = 2)]
    public sealed class AmmoData : ScriptableObject
    {
        [SerializeField] private BulletUnit _prefab;
        public BulletUnit Prefab => _prefab;
        
        [Tooltip("The number of bullets in the pool at the start of the games")]
        [SerializeField, Range(1, 100)] private int startSizePool = 15;
        public int SizePool => startSizePool;
        
        [Header("Слои взаимодействия")]
        [SerializeField] private string playerMask = "PlayerBullet";
        public string PlayerMask => playerMask;
        
        [SerializeField] private string enemyMask = "EnemyBullet";
        public string EnemyMask => enemyMask;
    }
}