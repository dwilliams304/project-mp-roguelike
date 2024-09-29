using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : NetworkBehaviour
{
    //INPUT SYSTEM
    // private PlayerControls playerControls;
    private PlayerInput playerInput;
    private InputAction movementAction, aimingAction;

    //Components
    // private Camera mainCamera;
    private Rigidbody rb;
    
    //Variables
    private Vector3 playerMovement;
    private Vector2 playerAiming;
    private bool usingController;

    //Serialized Variables
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float controllerDeadzone;
    [SerializeField] private float controllerAimSmoothing;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioListener audioListener;


    private void Awake(){
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        // playerControls = new PlayerControls();
        // mainCamera = GetComponent<Camera>();

    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            audioListener.enabled = false;
            virtualCamera.Priority = 0;
            return;
        }

        movementAction = playerInput.actions["Movement"];
        aimingAction = playerInput.actions["Aim"];
        audioListener.enabled = true;
        virtualCamera.Priority = 100;

                // mainCamera = GetComponent<Camera>();
        
    }

    // private void OnEnable(){
    //     playerControls.Enable();
    // }
    // 
    // private void OnDisable(){
    //     playerControls.Disable();
    // }


    private void Update()
    {
        if(!IsOwner) return;
        HandleMovement();
        HandleAiming();
    }

    
    private void HandleMovement(){
        playerMovement = movementAction.ReadValue<Vector2>();

        // rb.velocity = playerMovement * 5f;
        rb.velocity = new Vector3(playerMovement.x, 0f, playerMovement.y) * 5f;
    }

    private void HandleAiming(){
        playerAiming = aimingAction.ReadValue<Vector2>();

        if(usingController){
            //Handle aiming with controller
            if(Mathf.Abs(playerAiming.x) > controllerDeadzone || Mathf.Abs(playerAiming.y) > controllerDeadzone){
                Vector3 dir = Vector3.right * playerAiming.x + Vector3.forward * playerAiming.y;
                if(dir.sqrMagnitude > 0f){
                    transform.forward = dir;
                }
            }
        }
        else{

            Ray ray = mainCamera.ScreenPointToRay(playerAiming);

            if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundLayer)){
                Vector3 dir = hitInfo.point - transform.position;
                dir.y = 0;
                transform.forward = dir;
            }
        }

    }

    public void OnControllerSwitch(PlayerInput input) => usingController = input.currentControlScheme.Equals("Controller") ? true : false;
}
