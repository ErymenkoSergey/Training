using Modules.UI;
using Modules.Utils;
using UnityEngine;
using Game.Interfaces;

namespace Game.Mechanics.Ship
{
    public sealed class UIHandler : MonoBehaviour
    {
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private GameOverView _gameOverView;

        private IGameUIStatus iGameUIStatus;

        public void Construct(IGameUIStatus iStatus)
        {
            iGameUIStatus = iStatus;
            iGameUIStatus.OnChangeScore += ChangeScore;
            iGameUIStatus.OnChangeHealth += ChangeHealth;
            iGameUIStatus.OnShowGameOverPanel += GameOver;
        }

        private void OnDestroy()
        {
            iGameUIStatus.OnChangeScore -= ChangeScore; //?
            iGameUIStatus.OnChangeHealth -= ChangeHealth;
            iGameUIStatus.OnShowGameOverPanel -= GameOver;
        }

        private void ChangeHealth(int health, int maxHealth)
        {
            _healthView.SetHealth(health, maxHealth);
            _cameraShaker.Shake();
        }

        private void ChangeScore(int value) => scoreView.SetValue(value);

        private void GameOver() => _gameOverView.Show();
    }
}