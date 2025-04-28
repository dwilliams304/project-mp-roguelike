using UnityEngine;

namespace ContradictiveGames
{
    public class AttackData : ScriptableObject
    {
        [Header("UI Settings")]
        public string AttackName;
        public Sprite AttackIcon;
        
        [Header("Attack Stats")]
        public int Damage;
        public float AttackCooldown;
        public int BaseCritChance;
        public float CritMultiplier = 2f;

        [Header("Effects")]
        public AudioClip AttackSound;
        
    }
}