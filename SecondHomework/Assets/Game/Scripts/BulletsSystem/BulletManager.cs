using Game.Mechanics.BulletsSystem.Data;
using Modules.Utils;
using UnityEngine;
using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine.Serialization;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletManager : MonoBehaviour, IBulletSpawner, IPool<Bullet>
    {
        [FormerlySerializedAs("bulletSystemConfig")] [Header("Data")] [SerializeField] private BulletSystemConfiguration bulletSystemConfiguration;
        [SerializeField] private Transform container;
        [SerializeField] private TransformBounds levelBounds;
        private readonly Stack<Bullet> bulletPool = new();

        public void Awake()
        {
            if (bulletSystemConfiguration == null)
            {
                Debug.LogError("No Data Configuration SO");
                return;
            }

            bulletSystemConfiguration.SetReferences(levelBounds);

            for (var i = 0; i < bulletSystemConfiguration.SizePool; i++)
            {
                Bullet bullet = Instantiate(bulletSystemConfiguration.CreateBullet(), container);
                bullet.gameObject.SetActive(false);
                bulletPool.Push(bullet);
            }
        }

        public void Spawn(BulletNavigation config)
        {
            if (bulletPool.TryPop(out Bullet bullet))
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(bulletSystemConfiguration.CreateBullet(), container);
            bullet.SetData(this, bulletSystemConfiguration.VFXConfiguration, config);
        }

        public void Return(Bullet enemy)
        {
            enemy.gameObject.SetActive(false);
            bulletPool.Push(enemy);
        }
    }
}