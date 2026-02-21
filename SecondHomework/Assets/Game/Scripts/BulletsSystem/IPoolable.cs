using Game.Mechanics.BulletsSystem;

namespace Game.Interfaces
{
    public interface IPoolable
    {
        void ReturnToPool(BulletUnit bullet, bool isUseVfx = false);
    }
}