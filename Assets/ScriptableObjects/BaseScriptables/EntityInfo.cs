using UnityEngine;


public enum UnitType {
    Melee,
    Ranged,
    Hybrid
}

public enum DamageType {
    Physical,
    Arcane,
    Light,
    Shadow
}


[CreateAssetMenu(fileName = "Entity Info", menuName = "Stats/Entity Info")]
public class EntityInfo : ScriptableObject
{
    [Header("Base Entity Traits/Stats")]
    public UnitType unitType = UnitType.Melee;
    public DamageType damageType = DamageType.Physical;

    [Header("Movement")]
    public float MoveSpeed = 7f;


    [Header("Health")]
    [Tooltip("Base Max Health")]
    public int Health = 100;
    public bool canRegenHealth = false;
    [Tooltip("How much player health will increase on tick")]
    public int HealthRegenAmount = 0;
    [Tooltip("How often player health regeneration will tick (as seconds)")]
    public float HealthRegenSpeed = 0f;

    [Header("Attack")]
    public int AttackPower = 10; //Base damage = 10
    [Tooltip("Chance to land attack")]
    public int HitChance = 100; //100% chance for every attack to hit

    [Header("Player Power")]    
    [Tooltip("Max Mana/Energy")]
    public int Power = 100;
    [Tooltip("If the class uses mana or energy")]
    public bool UsesMana = false; //Uses energy by default
    [Tooltip("How much player power will increase by on tick")]
    public int PowerRegenAmount = 1; //Will gain + 1 every tick
    [Tooltip("How often player power regeneration will tick (as seconds)")]
    public float PowerRegenRate = 2f; //Will tick every 2 seconds

    [Header("Critical Strike")]
    [Tooltip("How likely it is for the damage to critically hit (as %)")]
    public int CritChance = 15; //15% chance to crit
    [Tooltip("How much the damage stat will be multiplied upon crit (as %)")]
    public int CritDamageMultiplier = 300; // Increase damage by 300%




    [Header("Drops/Loot (as & increases)")]
    public int GoldDropModifier = 0; //0% increase to gold drops 
    public int XPDropModifier = 0; //0% increase to XP drops
    public int LuckModifier = 0; //0% luck increase


    [Header("Damage Resistances (as % decrease)")]
    public int PhysicalDamageResist = 0; //0% decrease to physical damage taken
    public int ArcaneDamageResist = 0; //0% decrease to arcane damage taken
    public int LightDamageResist = 0; //0% decrease to light damage taken
    public int ShadowDamageResist = 0; //0% decrease to shadow damage taken
}
