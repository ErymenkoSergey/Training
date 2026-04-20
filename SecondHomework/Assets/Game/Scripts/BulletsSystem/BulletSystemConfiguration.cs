using Game.Data.VFX;
using Game.Enums;
using Modules.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.BulletsSystem.Data
{
    [CreateAssetMenu(menuName = "Game/BulletSystemConfiguration", order = 2)]
    public sealed class BulletSystemConfiguration : ScriptableObject
    {
        [SerializeField] private Bullet prefab;
        [SerializeField] private BulletConfiguration enemyConfig;
        [SerializeField] private BulletConfiguration playerConfig;
        [FormerlySerializedAs("vfxData")] [SerializeField] private VFXConfiguration vfxConfiguration;
        public VFXConfiguration VFXConfiguration => vfxConfiguration;
        private TransformBounds levelBounds;
        [Tooltip("The number of bullets in the pool at the start of the games")] [SerializeField, Range(1, 100)]
        private int startSizePool = 15;

        public int SizePool => startSizePool;
        
        public void SetReferences(TransformBounds levelBounds) => this.levelBounds = levelBounds;
        
        public Bullet CreateBullet()
        {
            var bullet = prefab;
            bullet.Construct(levelBounds); 
            return bullet;
        }

        public BulletConfiguration GetBulletConfiguration(TeamType team)
        {
            switch (team)
            {
                case TeamType.Player:
                    return playerConfig;
                case TeamType.Enemy:
                    return enemyConfig;
                default:
                    return enemyConfig;
            }
        }
    }
}