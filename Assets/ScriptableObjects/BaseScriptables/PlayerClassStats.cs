using UnityEngine;



[CreateAssetMenu(fileName = "Class", menuName = "Stats/Player Class Stats")]
public class PlayerClassStats : BaseUnitStats
{
    [Header("Base Class Info")]
    public string ClassName = "Class";
    [Multiline]
    public string Description = "Class description";

    [Header("Player Power")]    
    [Tooltip("Max Mana/Energy")]
    public int Power = 100;
    [Tooltip("If the class uses mana or energy")]
    public bool UsesMana = false;
    [Tooltip("How much player power will increase by on tick")]
    public int PowerRegenAmount;
    [Tooltip("How often player power regeneration will tick (as seconds)")]
    public float PowerRegenRate;

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
