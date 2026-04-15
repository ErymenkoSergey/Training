using System;
using Game.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.Configuration
{
    [Serializable]
    public sealed class SoundConfiguration
    {
        [FormerlySerializedAs("_audioSource")] [SerializeField]
        private AudioSource audioSource;

        [SerializeField] private SoundData soundData;
        
        public void PlayFireSFX()
        {
            if (soundData)
                PlaySound(soundData.FireSFX);
        }

        public void PlayDamageSFX()
        {
            if (soundData)
                PlaySound(soundData.DamageSFX);
        }

        public void PlayDeadSFX()
        {
            if (soundData)
                PlaySound(soundData.DeadSFX);
        }

        private void PlaySound(AudioClip clip) => audioSource?.PlayOneShot(clip);
    }
}