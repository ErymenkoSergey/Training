using System;
using Game.Interfaces;
using Game.Mechanics.BulletsSystem;
using Game.Mechanics.Inputs;
using Game.Mechanics.Ship;
using Game.Mechanics.Spawner;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core
{
    public sealed class Gameplay : MonoBehaviour, IGameLoop // Нарушает срп 
    
    // вынести отдельный класс гейм луп (для этого OnGameOver)
    // 
    {
        public event Action<bool> OnGameOver;
        
        [SerializeField] private BaseShip playerShip;
        private Transform target => playerShip.transform;
        private IMovable iMovable => playerShip;
        private IShot iShot => playerShip;

        [SerializeField] private PlayerInput input;
        [SerializeField] private UIHandler uiHandler;
        private IViewHealth iViewHealth => uiHandler;
        private IScore iScore => uiHandler;

        [SerializeField] private EnemyController enemyController;

        [FormerlySerializedAs("manager")] [SerializeField] private BulletManager bulletManager;
        private IBulletSpawner IBulletSpawner => bulletManager;
        
        private void Awake()
        {
            SetRef();
        }

        private void SetRef() // отдельный компонент гейм инсталлер с этой логикой.  
        {
            playerShip.Construct(IBulletSpawner);
            input.Construct(iMovable, iShot, this);
            enemyController.Construct(IBulletSpawner, target, iScore);
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
            //iViewHealth.ChangeHealth(health, playerShip.CurrentMaxHealth);
        }

        private void GameOver()
        {
            OnGameOver?.Invoke(true);
            uiHandler.GameOver();
            playerShip.gameObject.SetActive(false);
        }
    }
}