using Game.Enums;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ExplosionVFXData", menuName = "Game/New ExplosionVFXData", order = 1)]
    public sealed class ExplosionVFXData : ScriptableObject
    {
        [SerializeField] private GameObject enemyExplosionVFX;
        [SerializeField] private GameObject playerExplosionVFX;
        [SerializeField] private GameObject blueVFX;
        public GameObject BlueVFX => blueVFX;
        [SerializeField] private GameObject redVFX;
        public GameObject RedVFX => redVFX;
        
        public void SpawnVFX(Transform point, TeamType team) // вынести в класс или so с визуальными эффектами?
        {
            GameObject prefab = team == TeamType.Enemy ? playerExplosionVFX : enemyExplosionVFX;
            Instantiate(prefab, point.position, prefab.transform.rotation);
        }
    }
}