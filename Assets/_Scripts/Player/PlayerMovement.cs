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
    private PlayerControls playerControls;
    private PlayerInput playerInput;

    private Camera mainCamera;
    
    private Vector3 playerMovement;
    private Vector2 playerAiming;

    private bool usingController;

    [SerializeField] private LayerMask groundLayer;

    private void Awake(){
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    private void Start(){

    }

    private void Update()
    {
        playerAiming = playerControls.Controls.Aim.ReadValue<Vector2>();
        HandleMovement();
        HandleAiming();
    }


    private void HandleMovement(){
        playerMovement = playerControls.Controls.Movement.ReadValue<Vector3>();

        characterController.Move(playerMovement * Time.deltaTime * 5f);
        
    }

    private void HandleAiming(){
        if(usingController){
            //Handle aiming with controller
        }
        else{
            Ray ray = mainCamera.ScreenPointToRay(playerAiming);

            if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundLayer)){
                var dir = hitInfo.point - transform.position;
                dir.y = 0;
                transform.forward = dir;
            }
        }

    }

    public void OnControllerSwitch(PlayerInput input) => usingController = input.currentControlScheme.Equals("Controller") ? true : false;
}
