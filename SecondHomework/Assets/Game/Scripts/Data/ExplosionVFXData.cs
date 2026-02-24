using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ExplosionVFXData", menuName = "Game/New ExplosionVFXData", order = 1)]
    public sealed class ExplosionVFXData : ScriptableObject
    {
        [SerializeField] private GameObject enemyExplosionVFX;
        public GameObject ExplosionVFX => enemyExplosionVFX;
        [SerializeField] private GameObject playerExplosionVFX;
        public GameObject PlayerExplosionVFX => playerExplosionVFX;
    }
}