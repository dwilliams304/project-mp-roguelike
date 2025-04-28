using UnityEngine;

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Ranged Attack", menuName = "Custom/Combat/Ranged Attack")]
    public class RangedAttack : AttackData
    {
        public bool IsProjectile;
        public float ProjectileSpeed;
        public float MaxDistance;
    }
}