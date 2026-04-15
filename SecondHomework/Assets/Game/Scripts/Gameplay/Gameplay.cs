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
    {
        public event Action OnFinished; 
        
        [SerializeField] private PlayerShip player;
        private BaseShip playerShip;
        private ITarget target => player;
        private IMovable iMovable;
        private IShot iShot;

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
            playerShip = player.Ship;
            iMovable = playerShip;
            iShot = playerShip;
            
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
            iViewHealth.ChangeHealth(health, playerShip.CurrentMaxHealth);
        }

        private void GameOver()
        {
            OnFinished?.Invoke();
            uiHandler.GameOver();
            playerShip.gameObject.SetActive(false);
        }
    }
}