using UnityEngine;

namespace ContradictiveGames
{

    public class AttackSO : ScriptableObject
    {
        public string AttackName = "Default Name";
        public Sprite AttackIcon;

        public float BaseDamageMin = 1f;
        public float BaseDamageMax = 3f;

        public float AttackCooldown = .1f;
        public float AttackRange = 2f;

        public virtual void OnAttackPerformed(){
            Debug.Log("Attack performed!");
        }
    }
}
