using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Inputs
{
    public sealed class PlayerInput : MonoBehaviour
    {
        private IMovable iMovable;
        private IShot iShot;
        private IGameOver iGameOver;
        
        private bool isGameOver;
        
        public void Construct(IMovable movable, IShot shot, IGameOver gameOver)
        {
            iMovable = movable;
            iShot = shot;
            iGameOver = gameOver;
            iGameOver.OnGameOver += SetGameOver;
        }

        private void OnDisable()
        {
            iGameOver.OnGameOver -= SetGameOver;
        }

        private void SetGameOver(bool isOver) => isGameOver = isOver;

        public void Update()
        {
            if (isGameOver)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                iShot.Fire(transform.up);

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            iMovable.ChangeDirection(new Vector2(dx, dy));
        }
    }
}