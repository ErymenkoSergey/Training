namespace Game.Interfaces
{
    public interface IPool<T>
    {
        void Return(T obj); // bool isUseVfx = false
    }
}