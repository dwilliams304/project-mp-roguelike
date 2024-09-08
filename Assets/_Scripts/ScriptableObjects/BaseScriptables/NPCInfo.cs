using UnityEngine;


[CreateAssetMenu(fileName = "NPC Info", menuName = "Stats/NPCs")]
public class NPCInfo : EntityInfo
{
    [Space(40)]
    [Header("START NPC INFO")]
    public bool isFriendly;
    public bool isDamageable;
    public bool isHealable;

    [Header("UI")]
    public bool usesHealthBar;
    public bool showsDamageText;
}
