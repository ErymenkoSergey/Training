using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Config
{
   public class EnemyConfiguration
   {
      public Vector3 SpawnPosition;
      public Vector3 AttackPosition;
      public ITarget Target;
      public IEnemyRespawn Respawn;
      public IShootable Shootable;
      public IGameOver GameOver;
   }
}