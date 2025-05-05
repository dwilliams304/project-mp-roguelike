using ContradictiveGames.Entities;

public interface IEntity
{
    public EntityData EntityData { get; }
    public int MaxHealth { get; }
    public int CurrentHealth { get; }

    public void DamageServerRpc(int amount);
    public void HealServerRpc(int amount);

    public bool IsHealable();
    public bool IsDamageable();

    public void Die();
}