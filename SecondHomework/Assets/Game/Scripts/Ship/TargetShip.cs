using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    // нарушение срп тк подразбить тк тут рабоата с ui и камерой  - слишком много ответственности , отключить от базового корабля
    public sealed class TargetShip : BaseShip, ITarget, IMovable, IShot
    {
        [SerializeField] private TransformBounds _playerArea;
        // private IViewHealth viewHealth;

        public void Construct(IShootable iShootable, IViewHealth viewHealth, IGameOver gameOver)
        {
            base.iShootable = iShootable;
            // this.viewHealth = viewHealth;
            base.gameOver = gameOver;
            base.StartShip();
        }

        // private void OnEnable()
        // {
        //     OnHealthChanged += ChangeHealth;
        //     OnDead += GameOver;
        // }
        //
        // private void OnDisable()
        // {
        //     OnHealthChanged -= ChangeHealth;
        //     OnDead -= GameOver;
        // }
        //
        // private void ChangeHealth(int health)
        // {
        //     viewHealth.ChangeHealth(health, CurrentMaxHealth);
        // }
        //
        // private void GameOver()
        // {
        //     //viewHealth.GameOver();
        //     gameOver.CallGameOver();
        //     gameObject.SetActive(false);
        // }

        #region Movement process

        public void ChangeDirection(Vector2 direction) => moveDirection = direction;

        public void Shot() => base.Fire(firePoint.up);

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