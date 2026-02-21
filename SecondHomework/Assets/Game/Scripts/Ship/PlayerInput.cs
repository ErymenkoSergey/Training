using Game.Interfaces;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Mechanics.Inputs
{
    public sealed class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerShip player;

        private IMovable iMovable;
        private IShootable iShootable;
        private IHealth ihealth;

        private void Awake()
        {
            if (player == null)
                Debug.LogError($" No Player !!");

            if (player.TryGetComponent(out IMovable movable))
                iMovable = movable;
            if (player.TryGetComponent(out IHealth health))
                ihealth = health;
            // if (player.TryGetComponent(out IShootable shootable))
            //     iShootable = shootable;
        }

        public void Update()
        {
            if (ihealth.CurrentHealth <= ihealth.DeadValueHealth)
                return;

            // if (Input.GetKeyDown(KeyCode.Space))
            //     iShootable.Shoot();

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            iMovable.ChangeDirection(new Vector2(dx, dy));
        }
    }
}