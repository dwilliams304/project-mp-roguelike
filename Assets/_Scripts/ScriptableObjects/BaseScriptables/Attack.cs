using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public string AttackName = "Base Attack";
    [Tooltip("How much damage the ability will use as % of attack power")]
    public int AttackPowerUsed = 100; //Will use 100% of base damage
    public float AttackCooldown = 0.5f;


    public virtual void OnAttack(){
        //Do something
    }
}
