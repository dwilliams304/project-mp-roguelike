using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction primaryAttackAction, secondaryAttackAction;



    private void Awake(){
        playerInput = GetComponent<PlayerInput>();

        primaryAttackAction = playerInput.actions["PrimaryAttack"];
        secondaryAttackAction = playerInput.actions["SecondaryAttack"];
    }
}
