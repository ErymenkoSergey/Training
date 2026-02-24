using Game.Mechanics.BulletsSystem.Data;
using Modules.Utils;
using UnityEngine;
using System.Collections.Generic;
using Game.Data;
using Game.Enums;
using Game.Interfaces;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletManager : MonoBehaviour, IShootable, IPoolable
    {
        [Header("Data")]
        [SerializeField] private AmmoData ammoData;
        [SerializeField] private ExplosionVFXData _configView;

        [SerializeField] private Transform container;
        [SerializeField] private TransformBounds levelBounds;

        private readonly Stack<BulletUnit> bulletPool = new();

        public void Awake()
        {
            if (ammoData == null || _configView == null)
            {
                Debug.LogError("No Data Configuration SO");
                return;
            }

            for (var i = 0; i < ammoData.SizePool; i++)
            {
                BulletUnit bullet = Instantiate(ammoData.Prefab, container);
                bullet.gameObject.SetActive(false);
                bulletPool.Push(bullet);
            }
        }

        public void Shoot(BulletConfiguration config)
        {
            if (config.Team == TeamType.None)
            {
                Debug.LogError($"Spawn bullet => team: {config.Team}");
                return;
            }

            if (bulletPool.TryPop(out BulletUnit bullet))
                bullet.gameObject.SetActive(true);
            else
                bullet = Instantiate(ammoData.Prefab, container);

            config.BulletNameMask = SetBulletType(config.Team);
            config.Bounds = levelBounds;
            config.Pool = this;

            bullet.SetData(config);
        }
        
        public void ReturnToPool(BulletUnit bullet, bool isUseVfx = false)
        {
            bullet.gameObject.SetActive(false);
            bulletPool.Push(bullet);
            if (isUseVfx)
                SpawnVFX(bullet.transform, bullet.GetTeam());
        }

        private string SetBulletType(TeamType team)
        {
            switch (team)
            {
                case TeamType.Player:
                    return ammoData.PlayerMask;
                case TeamType.Enemy:
                    return ammoData.EnemyMask;
                case TeamType.None:
                default:
                    Debug.LogError($"Spawn bullet => team: {team}");
                    break;
            }

            return string.Empty;
        }
        
        private void SpawnVFX(Transform point, TeamType team) // вынести в класс с визуальными эффектами
        {
            GameObject prefab = team == TeamType.Enemy ? _configView.PlayerExplosionVFX : _configView.ExplosionVFX;
            Instantiate(prefab, point.position, prefab.transform.rotation);
        }
    }
}