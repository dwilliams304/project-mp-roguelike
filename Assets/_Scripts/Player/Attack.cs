using UnityEngine;

namespace ContradictiveGames
{

    public enum AttackType { Melee, Ranged }
    public enum RangedAttackType { Projectile, HitScan }
    
    [System.Serializable]
    public class Attack
    {
        public string AttackName = "Default Name";
        public Sprite AttackIcon;
        
        public float BaseDamageMin = 1f;
        public float BaseDamageMax = 3f;
        public float AttackCooldown = .1f;


        public AttackType attackType = AttackType.Melee;

        //Melee Attack Settings
        public float SwingRadius = 2f;
        public float SwingLength = 1f;
        public float SwingSpeed = 0.2f;


        //Ranged Attack Settings
        public GameObject projectilePrefab;
        public float ProjectileSpeed = 30f;
        public RangedAttackType RangedAttackType = RangedAttackType.Projectile;
    }
}
