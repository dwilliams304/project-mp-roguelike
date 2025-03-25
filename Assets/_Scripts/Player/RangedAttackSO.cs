using UnityEngine;

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Ranged Attack", menuName = "Custom/Combat/Ranged Attack")]
    public class RangedAttackSO : AttackSO
    {
        public override void OnAttackPerformed()
        {
            Debug.Log("Ranged attack performed!");
        }
    }
}