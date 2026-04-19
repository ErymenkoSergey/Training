using Game.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data.VFX
{
    [CreateAssetMenu(fileName = "VFXConfiguration", menuName = "Game/New VFXConfiguration", order = 1)]
    public sealed class VFXConfiguration : ScriptableObject, Ivfx
    {
        [Header("Explosions")] [SerializeField]
        private float timeReturn = 3f;

        [FormerlySerializedAs("enemyExplosionVFX")] [SerializeField]
        private GameObject bulletExplosionVFX;

        [FormerlySerializedAs("playerExplosionVFX")] [SerializeField]
        private GameObject shipExplosionVFX;

        [Header("Bullet view")] [SerializeField]
        private GameObject blueVFX;

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

        public void SpawnBulletExplosionVFX(Transform point)
        {
            GameObject prefab = bulletExplosionVFX;
            Instantiate(prefab, point.position, prefab.transform.rotation);
        }

        public void SpawnShipExplosionVFX(Transform point)
        {
            GameObject prefab = shipExplosionVFX;
            Instantiate(prefab, point.position, prefab.transform.rotation);
        }
    }

    public interface Ivfx
    {
        GameObject GetFlickerVFX(TeamType team);
        void SpawnBulletExplosionVFX(Transform point);
    }
}