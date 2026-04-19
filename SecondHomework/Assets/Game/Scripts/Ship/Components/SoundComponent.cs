using System;
using Game.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mechanics.Components
{
    [Serializable]
    public sealed class SoundComponent
    {
        [FormerlySerializedAs("_audioSource")] [SerializeField]
        private AudioSource audioSource;

        [FormerlySerializedAs("soundData")] [SerializeField] private SoundConfiguration soundConfiguration;
        
        public void PlayFireSFX()
        {
            if (soundConfiguration)
                PlaySound(soundConfiguration.FireSFX);
        }

        public void PlayDamageSFX()
        {
            if (soundConfiguration)
                PlaySound(soundConfiguration.DamageSFX);
        }

        public void PlayDeadSFX()
        {
            if (soundConfiguration)
                PlaySound(soundConfiguration.DeadSFX);
        }

        private void PlaySound(AudioClip clip) => audioSource?.PlayOneShot(clip);
    }
}