using System;
using Game.Core;
using Game.Mechanics.Ship;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public sealed class EnemySpawnController
{
    private UniversalPool<Enemy> universalPool;
    
    [Header("Cooldown Settings")]
    [SerializeField, Range(0.1f, 10f)] private float minSpawnCooldown = 2;
    [SerializeField, Range(0.1f, 10f)] private float maxSpawnCooldown = 3;
    
    [SerializeField] private EnemyManager enemyManager;
    
    private float spawnCooldown;
    private float spawnTime;
    private bool isTargetDestroyed;

    public void Construct()
    {
        universalPool = new UniversalPool<Enemy>();
        ResetSpawnCooldown();
        enemyManager.Construct();
    }

    private void ResetSpawnCooldown()
    {
        spawnCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
        spawnTime = Time.fixedTime;
    }
    
    public void FixedUpdate()
    {
        if (isTargetDestroyed)
            return;

        float time = Time.fixedTime;
        if (time - spawnTime < spawnCooldown)
            return;

        Enemy enemy = GetFreeEnemy();
        enemyManager.CreateEnemy(enemy);
        ResetSpawnCooldown();
    }

    private Enemy GetFreeEnemy()
    {
        var enemy = universalPool.TryObj();
            
        if (enemy != null)
            enemy.gameObject.SetActive(true);

        return enemy;
    }
    
    public void SetTargetDestroyed() => isTargetDestroyed = true;

    public void ReturnShip(Enemy enemy) => universalPool.Enqueue(enemy);
}
