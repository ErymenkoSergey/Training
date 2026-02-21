using Modules.UI;
using Modules.Utils;
using UnityEngine;
using Game.Interfaces;

namespace Game.Mechanics.Ship
{
    public sealed class UIHandler : MonoBehaviour, IScore, IViewHealth
    {
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private GameOverView _gameOverView; // game processing

        public void ChangeHealth(int health, int maxHealth)
        {
            _healthView.SetHealth(health, maxHealth);
            _cameraShaker.Shake();
        }

        public void ChangeScore(int value) => scoreView.SetValue(value);

        public void GameOver() => _gameOverView.Show();
    }
}