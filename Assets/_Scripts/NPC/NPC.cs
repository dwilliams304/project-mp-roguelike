using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NPC : NetworkBehaviour
{
    [SerializeField] NPCInfo npcInfo;
    private Health health;

    private void Awake(){
        if(!npcInfo.isFriendly && npcInfo.isDamageable){ 
            //If this NPC is unfriendly and CAN be damaged, either use the Health component if it exists, or add a new one
            health = GetComponent<Health>() ? GetComponent<Health>() : gameObject.AddComponent(typeof(Health)) as Health;
            // if(GetComponent<Health>()){
            //     health = GetComponent<Health>();
            // }
            // else{
            //     health = gameObject.AddComponent(typeof(Health)) as Health;
            // }
            health.InitializeHealthSystem(npcInfo.Health, npcInfo.usesHealthBar, npcInfo.showsDamageText, npcInfo.isHealable);
        }
    }
}
