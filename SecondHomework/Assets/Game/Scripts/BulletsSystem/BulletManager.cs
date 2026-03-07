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
        [SerializeField] private ExplosionVFXData _configView;

        [SerializeField] private Transform container;
        [SerializeField] private TransformBounds levelBounds;

        private readonly Stack<Bullet> bulletPool = new(); // Сделать универсальный пул! отв-ть пула

        public void Awake()
        {
            if (bulletSystemConfig == null || _configView == null)
            {
                Debug.LogError("No Data Configuration SO");
                return;
            }

            for (var i = 0; i < bulletSystemConfig.SizePool; i++)
            {
                Bullet bullet = Instantiate(bulletSystemConfig.Prefab, container);
                bullet.gameObject.SetActive(false);
                bulletPool.Push(bullet);
            }
        }

        public void Spawn(BulletConfiguration config)
        {
            if (config.Team == TeamType.None)
            {
                Debug.LogError($"Spawn bullet => team: {config.Team}");
                return;
            }

            if (bulletPool.TryPop(out Bullet bullet))
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(bulletSystemConfig.Prefab, container);

            config.BulletNameMask = bulletSystemConfig.GetBulletType(config.Team);
            config.Bounds = levelBounds;
            config.Pool = this;

            bullet.SetData(config);
        }
        
        public void ReturnToPool(Bullet bullet, bool isUseVfx = false)
        {
            bullet.gameObject.SetActive(false);
            bulletPool.Push(bullet);
            if (isUseVfx)
                _configView.SpawnVFX(bullet.transform, bullet.GetTeam());
        }
    }
}