using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
    public sealed class SoundData : ScriptableObject
    {
        [SerializeField] private AudioClip fireSFX;
        public AudioClip FireSFX => fireSFX;

        [SerializeField] private AudioClip damageSFX;
        public AudioClip DamageSFX => damageSFX;

        [SerializeField] private AudioClip deadSFX;
        public AudioClip DeadSFX => deadSFX;
    }
}