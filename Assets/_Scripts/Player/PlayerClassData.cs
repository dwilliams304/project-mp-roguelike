using UnityEngine;

namespace ContradictiveGames.Player
{
    [CreateAssetMenu(fileName = "Player Class Data", menuName = "Custom/Player Class Data")]
    public class PlayerClassData : ScriptableObject
    {
        [Header("UI")]
        [Tooltip("Name of the class")]
        public string ClassName = "Default Class";
        [Multiline]
        [Tooltip("Class Lore/Description")]
        public string ClassDescription = "Description goes here";
        public Sprite ClassIcon;
        [Space(20)]



        [Header("Combat")]
        public int MaxHealth;
        [Min(0)] public float BaseMoveSpeed;
        public AttackData PrimaryAttack;
        public AttackData SecondaryAttack;

        [Header("Tuning")]
        public float DamageAura;
        public float HealAura;
    }
}