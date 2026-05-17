using Game.Data.VFX;
using Game.Enums;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem.Data
{
    public sealed class BulletFactory : MonoBehaviour
    {
        [SerializeField] private Bullet prefab;
        [SerializeField] private BulletConfiguration enemyConfig;
        [SerializeField] private BulletConfiguration playerConfig;
        [SerializeField] private VFXConfiguration vfxConfiguration;
        [SerializeField] private TransformBounds levelBounds;
        [Tooltip("The number of bullets in the pool at the start of the games")] [SerializeField, Range(1, 100)]
        private int startSizePool = 15;
        public int SizePool => startSizePool;
        
        public Bullet CreateBullet(TeamType team)
        {
            var bullet = prefab;
            bullet.Construct(team, levelBounds, vfxConfiguration, GetBulletConfiguration(team)); 
            return bullet;
        }

        private BulletConfiguration GetBulletConfiguration(TeamType team)
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