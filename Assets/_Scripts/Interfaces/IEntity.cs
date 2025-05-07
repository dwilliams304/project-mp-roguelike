using ContradictiveGames;
using ContradictiveGames.Entities;

public interface IEntity
{
    public EntityData EntityData { get; }
    public EntityUIController entityUIController { get; }

    public void InitializeUI(EntityData data);

    public void Die();
}