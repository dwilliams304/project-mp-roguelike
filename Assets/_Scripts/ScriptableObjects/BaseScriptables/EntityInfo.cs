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

public enum Faction {
    Humanoid,
    Critter,
    Beast,
    Undead,
    Spirit,
    Elemental,
    Unknown,
}


[CreateAssetMenu(fileName = "Basic Entity", menuName = "Stats/Basic Entity")]
public class EntityInfo : ScriptableObject
{
    [Header("Base Entity Traits/Stats")]
    public string EntityName = "Entity Name";
    public UnitType unitType = UnitType.Melee;
    public DamageType damageType = DamageType.Physical;
    public Faction entityFaction = Faction.Humanoid;

    [Header("Movement")]
    public float MoveSpeed = 7f;


    [Header("Health")]
    [Tooltip("Base Max Health")]
    public int Health = 100;
    public bool canRegenHealth = false;
    [Tooltip("How much health will increase on tick")]
    public int HealthRegenAmount = 0;
    [Tooltip("How often health regeneration will tick (as seconds)")]
    public float HealthRegenSpeed = 0f;

    [Header("Attack")]
    public int AttackPower = 10; //Base damage = 10
    [Tooltip("Chance to land attack")]
    public int HitChance = 100; //100% chance for every attack to hit



    [Header("Critical Strike")]
    [Tooltip("How likely it is for the damage to critically hit (as %)")]
    public int CritChance = 15; //15% chance to crit
    [Tooltip("How much the damage stat will be multiplied upon crit (as %)")]
    public int CritDamageMultiplier = 300; // Increase damage by 300%


    [Header("Damage Resistances (as % decrease)")]
    public int PhysicalDamageResist = 0; //0% decrease to physical damage taken
    public int ArcaneDamageResist = 0; //0% decrease to arcane damage taken
    public int LightDamageResist = 0; //0% decrease to light damage taken
    public int ShadowDamageResist = 0; //0% decrease to shadow damage taken
}
