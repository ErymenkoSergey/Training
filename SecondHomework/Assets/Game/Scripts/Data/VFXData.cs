using System.Collections.Generic;
using Game.Enums;
using Game.Interfaces;
using UnityEngine;

namespace Game.Data.VFX
{
    [CreateAssetMenu(fileName = "ExplosionVFXData", menuName = "Game/New ExplosionVFXData", order = 1)]
    public sealed class VFXData : ScriptableObject, Ivfx//, IPool<BulletExplosion>
    {
        [Header("Explosions")] [SerializeField]
        private float timeReturn = 3f;
        [SerializeField] private GameObject enemyExplosionVFX;
        [SerializeField] private GameObject playerExplosionVFX;
        
        [Header("Bullet view")]
        [SerializeField] private GameObject blueVFX; 
        [SerializeField] private GameObject redVFX;
        
        
        // private readonly Stack<BulletExplosion> explosionPool = new();
        
        
        public GameObject GetFlickerVFX(TeamType team)
        {
            switch (team)
            {
                case TeamType.Player:
                    return blueVFX;
                case TeamType.Enemy:
                    return redVFX;
            }
            
            return null;
        }
        
        public void SpawnVFX(Transform point, TeamType team)
        {
            GameObject prefab = team == TeamType.Enemy ? playerExplosionVFX : enemyExplosionVFX;
            //prefab.SetData(this, timeReturn);
            Instantiate(prefab, point.position, prefab.transform.rotation);
        }
        
        
        // public void Return(BulletExplosion obj)
        // {
        //     explosionPool.Push(obj);
        //     obj.gameObject.SetActive(false);
        // }
    }

    public interface Ivfx
    {
        GameObject GetFlickerVFX(TeamType team);
        void SpawnVFX(Transform point, TeamType team);
    }
}