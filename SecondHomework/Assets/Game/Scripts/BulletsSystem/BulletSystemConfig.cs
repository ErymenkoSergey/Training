using Game.Data.VFX;
using Game.Enums;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem.Data
{
    [CreateAssetMenu(menuName = "Game/BulletSystemConfig", order = 2)]
    public sealed class BulletSystemConfig : ScriptableObject
    {
        [SerializeField] private Bullet prefab;
        [SerializeField] private BulletConfiguration enemyConfig;
        [SerializeField] private BulletConfiguration playerConfig;
        [SerializeField] private VFXData vfxData;
        public VFXData VFXData => vfxData;
        private TransformBounds levelBounds;
        [Tooltip("The number of bullets in the pool at the start of the games")] [SerializeField, Range(1, 100)]
        private int startSizePool = 15;

        public int SizePool => startSizePool;
        
        public void SetReferences(TransformBounds levelBounds) => this.levelBounds = levelBounds;
        
        public Bullet CreateBullet()
        {
            var bullet = prefab;
            bullet.Initialize(levelBounds); 
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