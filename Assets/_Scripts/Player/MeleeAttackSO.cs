using UnityEngine;

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Melee Attack", menuName = "Custom/Combat/Melee Attack")]
    public class MeleeAttackSO : AttackSO
    {
        public override void OnAttackPerformed()
        {
            Debug.Log("Melee attack performed!");
        }
    }
}