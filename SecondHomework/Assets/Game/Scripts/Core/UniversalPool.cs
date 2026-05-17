using System.Collections.Generic;

namespace Game.Core
{
    public sealed class UniversalPool<T>
    {
        private readonly Queue<T> pool = new();
        public T TryObj() => pool.TryDequeue(out T obj1) ? obj1 : default;
        public void Enqueue(T obj) => pool.Enqueue(obj);
    }
}