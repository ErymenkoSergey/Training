using Game.Interface;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Mechanics.Inputs
{
    public sealed class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerShip player;

        private IMovable iPlayer;
        private IHealth ihealth;

        private void Awake()
        {
            if (player == null)
                Debug.LogError($" No Player !!");

            iPlayer = player;
            ihealth = player;
        }

        public void Update()
        {
            if (ihealth.CurrentHealth <= ihealth.DeadValueHealth)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                iPlayer.Fire();

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            iPlayer.ChangeDirection(new Vector2(dx, dy));
        }
    }
}