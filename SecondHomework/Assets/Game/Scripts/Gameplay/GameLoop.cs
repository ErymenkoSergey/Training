using System;
using Game.Interfaces;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Core
{
    public sealed class GameLoop : MonoBehaviour, IGameLoop//, IGameUIStatus, IScore
    {
        public event Action OnStart;
        public event Action OnFinished;
        // public event Action<int, int> OnChangeHealth;
        //
        // public event Action OnShowGameOverPanel;

        // [SerializeField] private UIHandler uiHandler;
        private IShipStatus iHealthPlayer;

        private void OnEnable()
        {
            StartGame();
        }

        private void StartGame()
        {
            OnStart?.Invoke();
            // OnShowGameOverPanel?.Invoke();
            iHealthPlayer.GetShip().SetActive(true);
        }
        
        
        private void GameOver()
        {
            OnFinished?.Invoke();
            // OnShowGameOverPanel?.Invoke();
            iHealthPlayer.GetShip().SetActive(false);
        }
        

        public void SetPlayerStatus(IShipStatus iHealthPlayer)
        {
            this.iHealthPlayer = iHealthPlayer;
            // iHealthPlayer.OnHealthChanged += ChangeHealth;
            // iHealthPlayer.OnDead += GameOver;
            // uiHandler.Construct(this);
        }

        // private void OnDisable()
        // {
        //     iHealthPlayer.OnHealthChanged -= ChangeHealth;
        //     iHealthPlayer.OnDead -= GameOver;
        // }
        //
        // private void ChangeHealth(int health, int maxHealth) => OnChangeHealth?.Invoke(health, maxHealth);

        
        //
        // 
    }
}