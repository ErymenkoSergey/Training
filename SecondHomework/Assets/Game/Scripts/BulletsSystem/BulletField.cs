using Game.Mechanics.BulletsSystem.Data;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletField : MonoBehaviour
    {
        [SerializeField] private Gunner gunner;
        public Transform Container;
        public TransformBounds LevelBounds;

        private void Awake()
        {
            if (gunner != null)
                gunner.Init(this);
        }

        private void FixedUpdate()
        {
            if (gunner != null)
                gunner.FixedUpdate();
        }
    }
}