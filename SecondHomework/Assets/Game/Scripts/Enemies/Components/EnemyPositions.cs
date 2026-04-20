using System;
using UnityEngine;
using Modules.Utils;

namespace Game.Mechanics.Components
{
    [Serializable]
    public sealed class EnemyPositions 
    {
        [Header("Points")] 
        [SerializeField] private Transform[] spawnPositions;
        [SerializeField] private Transform[] attackPositions;
        
        private int spawnIndex;
        private int attackIndex;

        public void Construct()
        {
            spawnPositions.Shuffle();
            attackPositions.Shuffle();
        }
        
        public Vector3 NextSpawnPosition()
        {
            if (spawnIndex >= spawnPositions.Length)
            {
                spawnPositions.Shuffle();
                spawnIndex = 0;
            }

            return spawnPositions[spawnIndex++].position;
        }

        public Vector3 NextDestination()
        {
            if (attackIndex >= attackPositions.Length)
            {
                attackPositions.Shuffle();
                attackIndex = 0;
            }

            return attackPositions[attackIndex++].position;
        }
    }
}