namespace Game.Interfaces
{
    public interface IHealth
    {
        int CurrentHealth { get; set; }
        int CurrentMaxHealth { get; }
        int DeadValueHealth { get; }
        void SetDamage(int health);
        void NotifyAboutDead();
    }
}