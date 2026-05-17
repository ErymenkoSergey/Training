using Game.Interfaces;
using Game.Mechanics.Ship;

namespace Game.Mechanics.Args
{
    public class EnemyConstruct
    {
        public ITarget Target;
        public IPool<Enemy> Respawn;
        public IBulletSpawner BulletSpawner;
    }
}