using UnityEngine;

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Melee Attack", menuName = "Custom/Combat/Ranged Attack")]
    public class MeleeAttack : AttackData
    {
        public float SwingLength;
        public float SwingSpeed;
        public float SwingRadius;
    }
}