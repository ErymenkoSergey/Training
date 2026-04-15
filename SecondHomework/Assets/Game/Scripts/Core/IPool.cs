namespace Game.Interfaces
{
    public interface IPool<T>
    {
        void Return(T enemy);
    }
}