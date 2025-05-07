using FishNet.Object.Synchronizing;

public interface IHealth
{
    public int GetMaxHealth();
    public int GetCurrentHealth();

    public bool IsHealable();
    public bool IsDamageable();

    public void Damage(int amount);
    public void Heal(int amount);
}