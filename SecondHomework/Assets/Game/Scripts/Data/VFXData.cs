using Game.Enums;
using UnityEngine;

namespace Game.Data.VFX
{
    [CreateAssetMenu(fileName = "ExplosionVFXData", menuName = "Game/New ExplosionVFXData", order = 1)]
    public sealed class VFXData : ScriptableObject, IFlickerVFX, ISpawnVFX
    {
        [Header("Explosions")]
        [SerializeField] private GameObject enemyExplosionVFX;
        [SerializeField] private GameObject playerExplosionVFX;
        
        [Header("Bullet view")]
        [SerializeField] private GameObject blueVFX; 
        [SerializeField] private GameObject redVFX;
        
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
            Instantiate(prefab, point.position, prefab.transform.rotation);
        }
    }

    public interface IFlickerVFX
    {
        GameObject GetFlickerVFX(TeamType team);
    }

    public interface ISpawnVFX
    {
        void SpawnVFX(Transform point, TeamType team);
    }
}