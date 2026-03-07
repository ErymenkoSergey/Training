using System;

namespace Game.Interfaces
{
    public interface IGameLoop
    {
        event Action<bool> OnGameOver;
        bool IsFinished { get; }// = OnGameOver
    }
}