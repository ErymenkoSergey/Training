using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Config
{
   public class EnemyConfiguration
   {
      public Vector3 SpawnPosition;
      public Vector3 AttackPosition;
      public IPlayer Target;
      public IHealth TargetHealth;
      public IEnemyRespawn Respawn;
   }
}