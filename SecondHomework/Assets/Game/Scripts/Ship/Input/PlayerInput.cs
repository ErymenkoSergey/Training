using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Inputs
{
    public sealed class PlayerInput : MonoBehaviour
    {
        private IMovable iMovable;
        private IShot iShot;
        private IGameLoop iGameLoop;

        private bool isFinished;
        
        public void Construct(IMovable movable, IShot shot, IGameLoop gameLoop)
        {
            iMovable = movable;
            iShot = shot;
            iGameLoop = gameLoop;
            iGameLoop.OnFinished += SetGameLoop;
        }

        private void OnDisable()
        {
            iGameLoop.OnFinished -= SetGameLoop;
        }

        private void SetGameLoop() => isFinished = true;

        public void Update()
        {
            if (isFinished)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                iShot.Fire(transform.up);

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            iMovable.ChangeDirection(new Vector2(dx, dy));
        }
    }
}