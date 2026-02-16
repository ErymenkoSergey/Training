namespace Game.Interface
{
    public interface IHealth
    {
        int CurrentHealth { get; set; }
        int CurrentMaxHealth { get; }
        int DeadValueHealth { get; }
        void NotifyAboutHealthChanged(int health);
        void NotifyAboutDead();
    }
}