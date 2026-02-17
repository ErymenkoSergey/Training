using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "ExplosionVFXEffectData", menuName = "Game/New ExplosionVFXEffectData", order = 1)]
    public sealed class ExplosionVFXEffectData : ScriptableObject
    {
        [field: SerializeField] public GameObject BlueVFX { get; private set; }
        [field: SerializeField] public GameObject RedVFX { get; private set; }
        [field: SerializeField] public GameObject ExplosionVFX { get; private set; }
        [field: SerializeField] public GameObject BigExplosionVFX { get; private set; }
    }
}