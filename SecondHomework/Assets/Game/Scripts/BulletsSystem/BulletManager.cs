using Game.Mechanics.BulletsSystem.Data;
using Modules.Utils;
using UnityEngine;
using System.Collections.Generic;
using Game.Data;
using Game.Enums;
using Game.Interfaces;
using UnityEngine.Serialization;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletManager : MonoBehaviour, IBulletSpawner
    {
        [FormerlySerializedAs("ammoData")]
        [Header("Data")]
        [SerializeField] private BulletSystemConfig bulletSystemConfig;
        // [SerializeField] private VFXData vFXData;

        [SerializeField] private Transform container; 
        [SerializeField] private TransformBounds levelBounds;

        private Pool<Bullet> pool;
        
        private IPool<Bullet> bulletPool;

        public void Awake()
        {
            if (bulletSystemConfig == null)
            {
                Debug.LogError("No Data Configuration SO");
                return;
            }

            pool = new Pool<Bullet>();
            bulletPool = pool;
            for (var i = 0; i < bulletSystemConfig.SizePool; i++)
            {
                Bullet bullet = Instantiate(bulletSystemConfig.CreateBullet(TeamType.Player), container);
                bullet.gameObject.SetActive(false);
                bullet.SetBounds(levelBounds);
                bulletPool.Return(bullet);
            }
        }

        public void Spawn(BulletNavigation config)
        {
            // if (config.Team == TeamType.None)
            // {
            //     Debug.LogError($"Spawn bullet => team: {config.Team}");
            //     return;
            // }

            // if (pool.TryPop(out Bullet bullet))
            if (bulletPool.GetFreeObject(out Bullet bullet))
                bullet.gameObject.SetActive(true);
            else
            {
                bullet = Instantiate(bulletSystemConfig.CreateBullet(config.Team), container);
                bullet.SetBounds(levelBounds);
            }
            
            bullet.SetData(config, bulletPool);
        }
        
        public void ReturnToPool(Bullet bullet, bool isUseVfx = false)
        {
            bullet.gameObject.SetActive(false);
            // bulletPool.Push(bullet);
            bulletPool.Return(bullet);
            // if (isUseVfx)
            //     vFXData.SpawnVFX(bullet.transform, bullet.GetTeam());
        }
        
    }
}