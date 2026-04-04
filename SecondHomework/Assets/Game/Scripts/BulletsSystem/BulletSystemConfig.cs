using Game.Data.VFX;
using Game.Enums;
using Modules.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.BulletsSystem.Data
{
    [CreateAssetMenu(menuName = "Game/BulletSystemConfig", order = 2)]
    public sealed class BulletSystemConfig : ScriptableObject
    {
        [SerializeField] private Bullet prefab;
        [SerializeField] private BulletConfiguration enemyConfig;
        [SerializeField] private BulletConfiguration playerConfig;
        [SerializeField] private VFXData vfxData;

        [Tooltip("The number of bullets in the pool at the start of the games")] [SerializeField, Range(1, 100)]
        private int startSizePool = 15;

        public int SizePool => startSizePool;
        
        public Bullet CreateBullet(TeamType team)
        {
            var bullet = prefab;
            var config = GetBulletConfiguration(team);
            bullet.Initialize(config, vfxData);
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