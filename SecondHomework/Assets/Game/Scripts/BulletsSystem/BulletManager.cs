using Game.Mechanics.BulletsSystem.Data;
using UnityEngine;
using Game.Core;
using Game.Enums;
using Game.Interfaces;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletManager : MonoBehaviour, IBulletSpawner
    {
        [Header("Data")]
        [SerializeField] private BulletFactory bulletFactory;
        [SerializeField] private Transform containerPlayerBullet;
        [SerializeField] private Transform containerEnemyBullet;
        private UniversalPool<Bullet> universalPoolPlayerBullet;
        private UniversalPool<Bullet> universalPoolEnemyBullet;

        public void Awake()
        {
            if (bulletFactory == null)
            {
                Debug.LogError("No Data Configuration SO");
                return;
            }
            
            universalPoolPlayerBullet = new UniversalPool<Bullet>();
            CreatePool(TeamType.Player, containerPlayerBullet, universalPoolPlayerBullet);
            
            universalPoolEnemyBullet = new UniversalPool<Bullet>();
            CreatePool(TeamType.Enemy, containerEnemyBullet, universalPoolEnemyBullet);
        }

        private void CreatePool(TeamType team, Transform container, UniversalPool<Bullet> pool)
        {
            for (var i = 0; i < bulletFactory.SizePool; i++)
            {
                Bullet bullet = Instantiate(bulletFactory.CreateBullet(team), container);
                bullet.gameObject.SetActive(false);
                pool.Enqueue(bullet);
            }
        }
        
        public void Spawn(BulletArgs config)
        {
            var bullet = GetPool(config.Team).TryObj();
            
            if (bullet != null)
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(bulletFactory.CreateBullet(config.Team), GetContainer(config.Team));
            
            bullet.SetArgs(config);
            bullet.OnDestroy += Return;
        }

        private void Return(Bullet bullet)
        {
            bullet.OnDestroy -= Return;
            bullet.gameObject.SetActive(false);
            GetPool(bullet.Team).Enqueue(bullet);
        }

        private Transform GetContainer(TeamType team)
        {
            switch (team)
            {
                case TeamType.Player:
                    return containerPlayerBullet;
                case TeamType.Enemy:
                    return containerEnemyBullet;
                default: return null;
            }
        }
        
        private UniversalPool<Bullet> GetPool(TeamType team)
        {
            switch (team)
            {
                case TeamType.Player:
                    return universalPoolPlayerBullet;
                case TeamType.Enemy:
                    return universalPoolEnemyBullet;
                default: return null;
            }
        }
    }
}