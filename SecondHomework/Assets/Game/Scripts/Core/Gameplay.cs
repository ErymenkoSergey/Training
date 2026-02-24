using System;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Inputs;
using Game.Mechanics.Ship;
using Game.Mechanics.Spawner;
using UnityEngine;

namespace Game.Core
{
    public sealed class Gameplay : MonoBehaviour, IGameOver
    {
        public event Action<bool> OnGameOver;
        
        [SerializeField] private PlayerShip playerShip;
        private Transform target => playerShip.transform;
        private IMovable iMovable => playerShip;
        private IShot iShot => playerShip;

        [SerializeField] private PlayerInput input;
        [SerializeField] private UIHandler uiHandler;
        private IViewHealth iViewHealth => uiHandler;
        private IScore iScore => uiHandler;

        [SerializeField] private EnemyController enemyController;

        [SerializeField] private BulletManager bulletManager;
        private IShootable iShootable => bulletManager;
        
        private void Awake()
        {
            SetRef();
        }

        private void SetRef()
        {
            playerShip.Construct(iShootable, this);
            input.Construct(iMovable, iShot, this);
            enemyController.Construct(iShootable, target, iScore, this);
        }
        
        private void OnEnable()
        {
            playerShip.OnHealthChanged += ChangeHealth;
            playerShip.OnDead += GameOver;
        }

        private void OnDisable()
        {
            playerShip.OnHealthChanged -= ChangeHealth;
            playerShip.OnDead -= GameOver;
        }

        private void ChangeHealth(int health)
        {
            iViewHealth.ChangeHealth(health, playerShip.CurrentMaxHealth);
        }

        private void GameOver()
        {
            OnGameOver?.Invoke(true);
            uiHandler.GameOver();
            playerShip.gameObject.SetActive(false);
        }
    }
}