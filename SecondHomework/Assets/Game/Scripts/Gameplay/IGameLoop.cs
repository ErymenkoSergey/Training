using System;

namespace Game.Interfaces
{
    public interface IGameLoop
    {
        event Action OnFinished;
    }
}