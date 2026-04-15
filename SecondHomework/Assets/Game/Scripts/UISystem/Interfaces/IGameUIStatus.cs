using System;

namespace Game.Interfaces
{
    public interface IGameUIStatus
    {
        event Action<int, int> OnChangeHealth; 
        event Action<int> OnChangeScore;
        event Action OnShowGameOverPanel;
    }
}