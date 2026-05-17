using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IShot
    {
        event Action<Vector3> OnShot;
        void Fire(Vector3 direction);
        float FireTime { get; set; }
        Transform FirePoint { get; }
    }
}