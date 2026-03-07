using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Config
{
   public class EnemyConfiguration
   {
      public Vector3 SpawnPosition;
      public Vector3 AttackPosition;
      public Transform Target;
      public IEnemyRespawn Respawn;
      public IBulletSpawner BulletSpawner;
      public IGameLoop GameLoop;
   }
}