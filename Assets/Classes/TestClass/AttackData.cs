using UnityEngine;

namespace ContradictiveGames
{
    public class AttackData : ScriptableObject
    {
        public int Damage;
        public float AttackCooldown;

        public int BaseCritChance;
        public int CritDamage;
    }
}