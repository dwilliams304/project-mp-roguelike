using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Controls playerControls;
    private PlayerInput playerInput;
    
    private Vector2 playerMovement;
    private Vector2 playerAiming;

    private bool usingController;

    private void Awake(){
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerControls = new Controls();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    private void Start(){

    }

    private void Update(){
        HandleMovement();
        HandleAiming();
    }


    private void HandleMovement(){

    }

    private void HandleAiming(){

    }
}
