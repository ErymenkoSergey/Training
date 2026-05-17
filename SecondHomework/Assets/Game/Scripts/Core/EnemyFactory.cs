using Game.Interfaces;
using Game.Mechanics.Args;
using Game.Mechanics.Ship;
using UnityEngine;

public sealed class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy prefab;
    
    private ITarget target;
    private IPool<Enemy> pool;
    private IBulletSpawner spawner;

    public void Construct(ITarget target, IPool<Enemy> pool, IBulletSpawner spawner)
    {
        this.target = target;
        this.pool = pool;
        this.spawner = spawner;
    }
    
    public Enemy CreateEnemy(Transform spawnPoint)
    {
        var enemy = Instantiate(prefab, spawnPoint);
        enemy.Construct(GetData());
        return enemy;
    }

    private EnemyConstruct GetData()
    {
        EnemyConstruct config = new EnemyConstruct();
        config.Target = target;
        config.Respawn = pool;
        config.BulletSpawner = spawner;
        return config;
    }
}
