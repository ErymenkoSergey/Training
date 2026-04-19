using System;
using Unity.Collections;
using UnityEngine;

namespace Game.Mechanics.Components
{
    [Serializable]
    public sealed class HealthComponent 
    {
        [SerializeField] private int currentHealth;
        private const int DEAD_VALUE_HEALTH = 0;
        
        [SerializeField,ReadOnly] public int CurrentMaxHealth;
        public int CurrentHealth => currentHealth;
        public bool IsDead { get; private set; }
        
        public void SetHealth(int health) => currentHealth = health;
        public void SetHealthMax(int maxHealth) => CurrentMaxHealth = maxHealth;
        
        public void TakeDamage(int damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, DEAD_VALUE_HEALTH, CurrentMaxHealth);
            
            if (currentHealth <= DEAD_VALUE_HEALTH)
                IsDead = true;
        }
    }
}
