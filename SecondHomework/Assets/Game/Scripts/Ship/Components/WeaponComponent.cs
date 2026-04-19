using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Components
{
    [Serializable]
    public sealed class WeaponComponent : IShot
    {
        public event Action<Vector3> OnShot;

        [Header("Combat")] [SerializeField] private Transform firePoint;

        public Transform FirePoint
        {
            get { return firePoint; }
        }

        public float FireTime { get; set; } = 0f;

        public void Fire(Vector3 direction) => OnShot?.Invoke(direction);
    }
}