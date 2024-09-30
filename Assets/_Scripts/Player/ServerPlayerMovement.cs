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
        
        //Variables
        private bool usingController;

        //Serialized Variables
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float controllerDeadzone;
        [SerializeField] private float controllerAimSmoothing;

        [SerializeField] private Transform playerTransform;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private AudioListener audioListener;


        //Network Variables
        private int tick = 0;
        private float tickRate = 1f / 60f;
        private float tickDeltaTime = 0;

        private const int buffer = 1024;

        private HandleStates.InputState[] inputStates = new HandleStates.InputState[buffer];
        private HandleStates.TransformStateReadWrite[] transformStates = new HandleStates.TransformStateReadWrite[buffer];

        public NetworkVariable<HandleStates.TransformStateReadWrite> currentServerTransformState = new();
        public HandleStates.TransformStateReadWrite previousTransformState;



        private void Awake(){
            playerInput = GetComponent<PlayerInput>();
            charController = GetComponent<CharacterController>();
            movementAction = playerInput.actions["Movement"];
            aimingAction = playerInput.actions["Aim"];

        }

        private void OnEnable(){
            currentServerTransformState.OnValueChanged += OnServerStateChanged;
        }
        private void OnDisable(){
            currentServerTransformState.OnValueChanged -= OnServerStateChanged;
        }



        public override void OnNetworkSpawn(){
            if(IsOwner){
                audioListener.enabled = true;
                virtualCamera.Priority = 3;
            }
            else{
                virtualCamera.Priority = 0;
            }
            
        }



        private void Update(){
            Vector2 moveInput = movementAction.ReadValue<Vector2>();
            Vector2 playerAiming = aimingAction.ReadValue<Vector2>();
            
            if(IsClient && IsLocalPlayer){
                ProcessLocalPlayerMovement(moveInput, playerAiming);
            }
            else {
                SimulateOtherPlayers();
            }
        }

        

        private void HandleMovement(Vector2 input){
            Vector3 movement = input.x * playerTransform.right + input.y * playerTransform.forward;
            charController.Move(movement * 8f * tickRate);
        }



        private void HandleAiming(Vector2 aimInput){
            if(usingController){
                //Handle aiming with controller
                if(Mathf.Abs(aimInput.x) > controllerDeadzone || Mathf.Abs(aimInput.y) > controllerDeadzone){
                    Vector3 dir = Vector3.right * aimInput.x + Vector3.forward * aimInput.y;
                    if(dir.sqrMagnitude > 0f){
                        playerTransform.forward = dir;
                    }
                }
            }
            else{
                Ray ray = mainCamera.ScreenPointToRay(aimInput);

                if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundLayer)){
                    Vector3 dir = hitInfo.point - playerTransform.position;
                    dir.y = 0;
                    playerTransform.forward = dir;
                }
            }
        }



        public void OnControllerSwitch(PlayerInput input) => usingController = input.currentControlScheme.Equals("Controller") ? true : false;



        private void ProcessLocalPlayerMovement(Vector2 moveInput, Vector2 aimInput){
            tickDeltaTime += Time.deltaTime;

            if(tickDeltaTime > tickRate){
                int bufferIdx = tick % buffer;

                MovePlayerWithServerTickServerRPC(tick, moveInput, aimInput);
                HandleMovement(moveInput);
                HandleAiming(aimInput);

                HandleStates.InputState _inputState = new(){
                    Tick = tick,
                    MoveInput = moveInput,
                    AimInput = aimInput
                };

                HandleStates.TransformStateReadWrite _transformState = new(){
                    Tick = tick,
                    FinalPos = transform.position,
                    FinalRot = transform.rotation,
                    IsMoving = true
                };

                inputStates[bufferIdx] = _inputState;
                transformStates[bufferIdx] = _transformState;

                tickDeltaTime -= tickRate;
                if(tick == buffer){
                    tick = 0;
                }
                else {
                    tick++;
                }
            }
        }



        private void SimulateOtherPlayers(){
            tickDeltaTime += Time.deltaTime;

            if(tickDeltaTime > tickRate){
                if(currentServerTransformState.Value.IsMoving){
                    transform.position = currentServerTransformState.Value.FinalPos;
                    transform.rotation = currentServerTransformState.Value.FinalRot;
                }

                tickDeltaTime -= tickRate;
                if(tick == buffer) tick = 0;
                else tick++;
            }
        }



        [ServerRpc]
        private void MovePlayerWithServerTickServerRPC(int tick, Vector2 moveInput, Vector2 aimInput){
            HandleMovement(moveInput);
            HandleAiming(aimInput);

            HandleStates.TransformStateReadWrite _transformState = new(){
                Tick = tick,
                FinalPos = transform.position,
                FinalRot = transform.rotation,
                IsMoving = true
            };

            previousTransformState = currentServerTransformState.Value;
            currentServerTransformState.Value = _transformState;
        }



        private void OnServerStateChanged(HandleStates.TransformStateReadWrite previousVal, HandleStates.TransformStateReadWrite newVal){
            previousTransformState = previousVal;
        }
    }
}