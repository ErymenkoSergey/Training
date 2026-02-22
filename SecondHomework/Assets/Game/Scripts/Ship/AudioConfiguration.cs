using System;
using UnityEngine;

namespace Game.Mechanics.Configuration
{
    [Serializable]
    public sealed class SoundConfiguration
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _fireSFX; // урать в конфигурацию so
        [SerializeField] private AudioClip _damageSFX;

        public void PlayFireSFX()
        {
            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX); // перенести в метод эффекты. и  вызывать их от туда
        }

        public void PlayDamageSFX()
        {
            if (_damageSFX)
                _audioSource.PlayOneShot(_damageSFX);
        }
    }
}