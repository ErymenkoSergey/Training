using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

public sealed class Pool<T> : IPool<T>
{
    //enemy
    // private readonly Queue<Enemy> pool = new(); // логика идет в общий класс 
    //     if (pool.TryDequeue(out Enemy enemy)) //  вытаскивает свободный объект
    // pool.Enqueue(enemy);// помещает обратно в пул 
    //
    // //BulletManager
    //
    // private readonly Stack<Bullet> bulletPool = new(); // Сделать универсальный пул! отв-ть пула
    // private IPool<GameObject> poolImplementation;
    //
    // bulletPool.Push(bullet); // это добавляет в стек на старте игры 
    // if (bulletPool.TryPop(out Bullet bullet)) // вытаскивает свободный объект
    // bulletPool.Push(bullet);  // помещает обратно в пул 
    
    private readonly Stack<T> poolT = new();
    
    public void Return(T obj) => poolT.Push(obj);
    
    public bool GetFreeObject(out T obj1)
    {
        if (poolT.TryPop(out T obj))
        {
            obj1 = obj;
            return true;
        }
        else
        {
            obj1 = default;
            return false;
        }
    }
}
