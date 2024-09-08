using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {
    Melee,
    Ranged,
    Hybrid
}

public enum PrimaryDamageType {
    Physical,
    Arcane,
    Light,
    Shadow
}

[CreateAssetMenu(fileName = "Unit Stat", menuName = "Stats/Unit Stats")]
public class BaseUnitStats : ScriptableObject
{
    [Header("Base Unit Traits/Stats")]
    public UnitType unitType = UnitType.Melee;
    public PrimaryDamageType primaryDamageType = PrimaryDamageType.Physical;

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

    [Header("Damage")]
    public int Damage = 10; 
    [Tooltip("Chance to land attack")]
    public int HitChance = 100; //100% chance for every attack to hit
}
