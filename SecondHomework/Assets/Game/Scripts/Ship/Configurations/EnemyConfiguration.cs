using Game.Interfaces;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Mechanics.Config
{
   public class EnemyConfiguration
   {
      public Vector3 SpawnPosition;
      public Vector3 AttackPosition;
      public Transform Target;
      public IPool<Enemy> Respawn;
      public IBulletSpawner BulletSpawner;
   }
}