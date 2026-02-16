using Game.Interface;
using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    public sealed class PlayerShip : BaseShip, IPlayer
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

        private void ChangeHealth(int health)
        {
            _healthView.SetHealth(health, this.config.Health);
            _cameraShaker.Shake();
        }

        private void GameOver()
        {
            _gameOverView.Show();
        }

        public void Update() //go to input system
        {
            if (Input.GetKeyDown(KeyCode.Space))
                this.Fire();

            float dx = Input.GetAxisRaw("Horizontal"); // Go to input service// 
            float dy = Input.GetAxisRaw("Vertical");
            this.moveDirection = new Vector2(dx, dy);

            if (CurrentHealth > 0)
            {
                _motor.MoveStep(this.moveDirection);
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            this.transform.position = _playerArea.ClampInBounds(this.transform.position);
        }
    }
}