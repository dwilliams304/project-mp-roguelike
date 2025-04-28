using UnityEngine;

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Melee Attack", menuName = "Custom/Combat/Melee Attack")]
    public class MeleeAttack : AttackData
    {
        public float SwingLength;
        public float SwingSpeed;
        public float SwingArc;
    }
}