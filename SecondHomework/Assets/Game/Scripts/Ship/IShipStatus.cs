using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IShipStatus
    {
        event Action<int, int> OnHealthChanged;
        event Action OnDead;
        GameObject GetShip();
    }
}