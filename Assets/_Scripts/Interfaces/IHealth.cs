public interface IHealth
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }

    public bool IsHealable();
    public bool IsDamageable();

    public void Damage(int amount);
    public void Heal(int amount);
}