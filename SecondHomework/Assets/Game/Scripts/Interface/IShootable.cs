using Game.Mechanics.BulletsSystem;

namespace Game.Interfaces
{
    public interface IShootable
    {
        void Shoot(BulletConfiguration config);
    }
}