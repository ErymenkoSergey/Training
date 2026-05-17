using Game.Interfaces;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Mechanics.Spawner
{
    public sealed class EnemyController : MonoBehaviour, IPool<Enemy>
    {
        [SerializeField] private EnemySpawnController spawnController;
        [SerializeField] private EnemyFactory factory;
        
        private int destroyedEnemies;
        private ITarget target;
        private IScore score;
        private IGameLoop gameLoop;

        public void Construct(ITarget target, IScore score, IBulletSpawner bulletSpawner)
        {
            this.target = target;
            this.score = score;
            factory.Construct(target,this, bulletSpawner);
            StartSystem();
        }

        public void StartSystem() // запускать в евента on start game 
        {
            spawnController.Construct();
            score.ChangeScore(destroyedEnemies);
            target.OnDestroyed += TargetDestroy;
        }

        private void FixedUpdate() => spawnController.FixedUpdate();
        
        public void Return(Enemy enemy)
        {
            UpdateScore();
            enemy.gameObject.SetActive(false);
            spawnController.ReturnShip(enemy);
        }

        private void UpdateScore()
        {
            destroyedEnemies++;
            score.ChangeScore(destroyedEnemies);
        }

        private void TargetDestroy()
        {
            spawnController.SetTargetDestroyed();
            target.OnDestroyed -= TargetDestroy;
        }
    }
}