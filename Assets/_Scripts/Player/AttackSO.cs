using UnityEngine;

namespace ContradictiveGames
{

    public enum AttackType { Melee, Ranged }
    
    [CreateAssetMenu(fileName = "Attack Scriptable", menuName = "Custom/Combat/Attack Scriptable")]
    public class AttackSO : ScriptableObject
    {
        public string AttackName = "Default Name";
        public Sprite AttackIcon;

        public float BaseDamageMin = 1f;
        public float BaseDamageMax = 3f;

        public float AttackCooldown = .1f;

        public AttackType attackType = AttackType.Melee;


        [Header("Ranged Attack Settings")]
        public GameObject projectilePrefab;
        public float ProjectileSpeed = 30f;
    }
}
