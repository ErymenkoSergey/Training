using Game.Mechanics.BulletsSystem;

namespace Game.Interfaces
{
    public interface IBulletSpawner
    {
        void Spawn(BulletArgs config);
    }
}