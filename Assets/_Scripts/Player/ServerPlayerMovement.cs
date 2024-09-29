using System;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ContradictiveGames.Multiplayer.Entities.Players
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class ServerPlayerMovement : NetworkBehaviour
    {
        //INPUT SYSTEM
        private PlayerInput playerInput;
        private InputAction movementAction, aimingAction;
        private CharacterController charController;

        //Components
        // private Camera mainCamera;
        private Transform playerTransform;
        
        //Variables
        private bool usingController;

        //Serialized Variables
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float controllerDeadzone;
        [SerializeField] private float controllerAimSmoothing;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private AudioListener audioListener;


        //Network Variables
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();


        private void Awake(){
            playerInput = GetComponent<PlayerInput>();
            charController = GetComponent<CharacterController>();
            playerTransform = GetComponent<Transform>();
            movementAction = playerInput.actions["Movement"];
            aimingAction = playerInput.actions["Aim"];
            // rb = GetComponent<Rigidbody>();
            // playerControls = new PlayerControls();
            // mainCamera = GetComponent<Camera>();

        }

        public override void OnNetworkSpawn()
        {
            if(IsOwner){
                audioListener.enabled = true;
                virtualCamera.Priority = 3;
            }
            else{
                virtualCamera.Priority = 0;
            }
            
        }


        private void Update()
        {
            Vector2 moveInput = movementAction.ReadValue<Vector2>();
            Vector2 playerAiming = aimingAction.ReadValue<Vector2>();
            if(IsServer && IsLocalPlayer){
                HandleMovement(moveInput);
                HandleAiming(playerAiming);
            }
            else if(IsClient && IsLocalPlayer)
            {
                HandleMovementServerRPC(moveInput);
                HandleAimingServerRPC(playerAiming);
            }
        }

        
        private void HandleMovement(Vector2 input){
            Vector3 movement = input.x * transform.right + input.y * transform.forward;
            charController.Move(movement * 8f * Time.deltaTime);
        }

        [ServerRpc]
        private void HandleMovementServerRPC(Vector2 input){
            HandleMovement(input);
        }

        private void HandleAiming(Vector2 aimInput){
            if(usingController){
                //Handle aiming with controller
                if(Mathf.Abs(aimInput.x) > controllerDeadzone || Mathf.Abs(aimInput.y) > controllerDeadzone){
                    Vector3 dir = Vector3.right * aimInput.x + Vector3.forward * aimInput.y;
                    if(dir.sqrMagnitude > 0f){
                        transform.forward = dir;
                    }
                }
            }
            else{
                Ray ray = mainCamera.ScreenPointToRay(aimInput);

                if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundLayer)){
                    Vector3 dir = hitInfo.point - transform.position;
                    dir.y = 0;
                    transform.forward = dir;
                }
            }
        }
        
        [ServerRpc]
        private void HandleAimingServerRPC(Vector2 aimInput){
            HandleAiming(aimInput);
        }

        public void OnControllerSwitch(PlayerInput input) => usingController = input.currentControlScheme.Equals("Controller") ? true : false;
    }
}