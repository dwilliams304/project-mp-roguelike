using ContradictiveGames.Entities;

interface IRessurectable
{
    public bool IsRessurectable(EntityData data);
    public void RessurectServerRpc(IEntity entity);
}