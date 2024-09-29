namespace ContradictiveGames
{
    public interface IDamageable {
        public void OnDamage(int amount = 0, bool wasCrit = false);
    }
}