using System;

namespace Game.Interfaces
{
    public interface IGameOver
    {
        event Action<bool> OnGameOver;
    }
}