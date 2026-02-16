using UnityEngine;

namespace Game.Interface
{

    public interface IHealth
    {
        int CurrentHealth { get; set; }
        int CurrentMaxHealth { get; set; }
        void NotifyAboutHealthChanged(int health);
        void NotifyAboutDead();
    }
}