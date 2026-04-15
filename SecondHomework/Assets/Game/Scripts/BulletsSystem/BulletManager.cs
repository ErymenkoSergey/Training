using Game.Mechanics.BulletsSystem.Data;
using Modules.Utils;
using UnityEngine;
using System.Collections.Generic;
using Game.Interfaces;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletManager : MonoBehaviour, IBulletSpawner, IPool<Bullet>
    {
        [Header("Data")]
        [SerializeField] private BulletSystemConfig bulletSystemConfig;
        [SerializeField] private Transform container; 
        [SerializeField] private TransformBounds levelBounds;
        private readonly Stack<Bullet> bulletPool = new();

        public void Awake()
        {
            if (bulletSystemConfig == null)
            {
                Debug.LogError("No Data Configuration SO");
                return;
            }
            
            bulletSystemConfig.SetReferences(levelBounds);
            
            for (var i = 0; i < bulletSystemConfig.SizePool; i++)
            {
                Bullet bullet = Instantiate(bulletSystemConfig.CreateBullet(), container);
                bullet.gameObject.SetActive(false);
                bulletPool.Push(bullet);
            }
        }

        public void Spawn(BulletNavigation config)
        {
            if (bulletPool.TryPop(out Bullet bullet))
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(bulletSystemConfig.CreateBullet(), container);
            bullet.SetData(this, bulletSystemConfig.VFXData, config);
        }
        
        public void Return(Bullet enemy)
        {
            enemy.gameObject.SetActive(false);
            bulletPool.Push(enemy);
        }
    }
}