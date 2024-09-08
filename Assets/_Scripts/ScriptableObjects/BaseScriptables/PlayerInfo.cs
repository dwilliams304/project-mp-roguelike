using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Class", menuName = "Stats/Player Classes")]
public class PlayerInfo : EntityInfo
{
    [Space(40)]
    [Header("START PLAYER CLASS INFO")]

    [Header("Drops/Loot (as & increases)")]
    public int GoldDropModifier = 0; //0% increase to gold drops 
    public int XPDropModifier = 0; //0% increase to XP drops
    public int LuckModifier = 0; //0% luck increase

    [Header("Power")]    
    [Tooltip("Max Mana/Energy")]
    public int Power = 100;
    [Tooltip("If the class uses mana or energy")]
    public bool UsesMana = false; //Uses energy by default
    [Tooltip("How much power will increase by on tick")]
    public int PowerRegenAmount = 1; //Will gain + 1 every tick
    [Tooltip("How often power regeneration will tick (as seconds)")]
    public float PowerRegenRate = 2f; //Will tick every 2 seconds
}
