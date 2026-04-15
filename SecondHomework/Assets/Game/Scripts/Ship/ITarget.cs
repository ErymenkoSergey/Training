using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface ITarget
    {
        event Action OnDestroyed;
        Transform GetTransform();
    }
}