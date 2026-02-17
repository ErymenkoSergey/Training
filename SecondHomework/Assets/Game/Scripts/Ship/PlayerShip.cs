using Game.Interface;
using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class PlayerShip : BaseShip, IPlayer, IMovable
    {
        [SerializeField] private TransformBounds _playerArea;
        [SerializeField] private CameraShaker _cameraShaker;

        [Header("UI")] 
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private HealthView _healthView;

        private void OnEnable()
        {
            OnHealthChanged += ChangeHealth;
            OnDead += GameOver;
        }

        private void OnDisable()
        {
            OnHealthChanged -= ChangeHealth;
            OnDead -= GameOver;
        }

        #region UI process

        private void ChangeHealth(int health)
        {
            _healthView.SetHealth(health, CurrentMaxHealth);
            _cameraShaker.Shake();
        }

        private void GameOver() => _gameOverView.Show();

        #endregion

        #region Movement process

        public void ChangeDirection(Vector2 direction) => moveDirection = direction;

        public void Shoot() => base.Fire(firePoint.up);

        private void Update()
        {
            if (CurrentHealth <= DeadValueHealth)
                return;
            
            engine.MoveStep(this.moveDirection);
        }

        protected override void LateUpdate()
        {
            if (CurrentHealth <= DeadValueHealth)
                return;
            base.LateUpdate();
            transform.position = _playerArea.ClampInBounds(transform.position);
        }
        
        #endregion
    }
}