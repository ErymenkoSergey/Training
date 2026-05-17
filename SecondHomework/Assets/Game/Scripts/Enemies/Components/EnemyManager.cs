using System;
using Game.Mechanics.Args;
using Game.Mechanics.Components;
using Game.Mechanics.Ship;
using UnityEngine;

[Serializable]
public sealed class EnemyManager 
{
    [SerializeField] private Transform container;
    [SerializeField] private EnemyPositions positions;
    [SerializeField] private EnemyFactory factory;
    
    public void Construct() => positions.Construct();
    
    public void CreateEnemy(Enemy enemy)
    {
        if (enemy == null)
            enemy = factory.CreateEnemy(container);
        
        enemy.SetArgs(GetArgs());
    }
    
    private EnemyArgs GetArgs()
    {
        EnemyArgs config = new EnemyArgs();
        config.SpawnPosition = positions.NextSpawnPosition();
        config.AttackPosition = positions.NextDestination();
        return config;
    }
}
