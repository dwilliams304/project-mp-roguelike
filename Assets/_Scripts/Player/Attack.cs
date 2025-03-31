using UnityEngine;

namespace ContradictiveGames
{

    public enum AttackType { Melee, Ranged }
    public enum RangedAttackType { Projectile, HitScan }
    
    [System.Serializable]
    public class Attack
    {
        [Header("UI")]
        public string AttackName = "Default Name";
        public Sprite AttackIcon;
        
        [Header("Base Stats")]
        public float BaseDamageMin = 1f;
        public float BaseDamageMax = 3f;
        public float AttackCooldown = .1f;


        public AttackType attackType = AttackType.Melee;


        [Header("Ranged Attack Settings")]
        public GameObject projectilePrefab;
        public float ProjectileSpeed = 30f;
        public RangedAttackType RangedAttackType = RangedAttackType.Projectile;
    }
}
