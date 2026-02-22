using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ExplosionVFXData", menuName = "Game/New ExplosionVFXData", order = 1)]
    public sealed class ExplosionVFXData : ScriptableObject
    {
        [field: SerializeField] public GameObject ExplosionVFX { get; private set; }
        [field: SerializeField] public GameObject BigExplosionVFX { get; private set; }
    }
}