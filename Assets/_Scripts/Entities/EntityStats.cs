using ContradictiveGames.Systems.Stats;
using UnityEngine;


public enum EntityFaction {
    Player,
    Undead,
    Beast,
    Critter,
    Demon,
    Humanoid
}


namespace ContradictiveGames.Entities
{
    [CreateAssetMenu(fileName = "Class Stats", menuName = "Custom/Entities/Stats")]
    public class EntityStats : ScriptableObject
    {
        [Header("Health Related")]
        public Stat MaxHealth;
        [Tooltip("How long do we wait before we last took damage, and start regenerating health?")]
            public float HealthRegenWait;
        [Tooltip("How long we wait in between health ticks")]
            public float HealthRegenSpeed;
        
        [Header("Combat Related")]
        public Stat BaseDamage;
        public Stat PrimaryAttackSpeed;
        public Stat SecondaryAttackSpeed;
        public Stat CriticalStrikeChance;
        public Stat CriticalStrikeDamage;
        public Stat DamageTaken;
        public Stat HitChance;
        
        public Stat MoveSpeed;
    }
}
