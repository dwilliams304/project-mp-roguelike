using System;
using ContradictiveGames.Input;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player), typeof(PlayerCombat))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private InputReader inputReader;
        private Player player;
        private PlayerCombat combat;

        private Vector2 moveInput;
        private Vector2 mousePosition;
        private Vector3 lookTarget;

        private Animator animator;

        [SerializeField] private Transform playerModelTransform;


        [SerializeField] private CinemachineCamera cmVCam;
        [SerializeField] private Camera playerCamera;



        [Header("Look Rotation Settings")]
        [Range(0, 3)] [SerializeField] private float lookSmoothing = 0.15f;

        [Header("Other Settings")]
        [SerializeField] private float interactionRadius;

        private NetworkVariable<Quaternion> playerRotation = new NetworkVariable<Quaternion>(
            Quaternion.identity,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );


        [Header("TEST SETTINGS")]
        [SerializeField] private float playerMovementSpeed = 3f;


#region Initialization/De-initialization

        public override void OnNetworkSpawn()
        {
            if(!IsOwner){
                cmVCam.enabled = false;
                playerCamera.enabled = false;
                playerCamera.GetComponent<AudioListener>().enabled = false;
            }
            else{
                cmVCam.enabled = true;
                playerCamera.enabled = true;
                cmVCam.Priority = 100;
                cmVCam.Follow = transform;
                cmVCam.LookAt = transform;
                Init();
            }
        }

        /// <summary>
        /// Initialize player actions, input actions, subscribe to events....
        /// </summary>
        private void Init(){
            player = GetComponent<Player>();
            combat = GetComponent<PlayerCombat>();
            animator = GetComponentInChildren<Animator>();
            

            inputReader.Move += MoveDirection => moveInput = MoveDirection;
            inputReader.Look += LookDirection => mousePosition = LookDirection;

            // inputReader.MainAttack += combat.DoPrimaryAttack;
            // inputReader.SecondaryAttack += combat.DoSecondaryAttack;

            inputReader.EnablePlayerActions();

        }



        public override void OnNetworkDespawn()
        {
            if(IsOwner){
                DeInit();
            }
            
        }

        /// <summary>
        /// Deinitialize player actions, unsubscribe to events, etc...
        /// </summary>
        private void DeInit(){
            inputReader.Move -= MoveDirection => moveInput = MoveDirection;
            inputReader.Look -= LookDirection => mousePosition = LookDirection;

            // inputReader.MainAttack -= combat.DoPrimaryAttack;
            // inputReader.SecondaryAttack -= combat.DoSecondaryAttack;
        }


#endregion


#region Core Loop

        private void Update()
        {
            if(IsOwner) {
                if(moveInput != Vector2.zero){
                    MoveCharacter(CalculateMoveDirection());
                }
                else{
                    animator.SetBool("IsRunning", false);
                }
                RotateCharacter(CalculatePlayerRotation());            
            }
            else{
                playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, playerRotation.Value, lookSmoothing);
            }
        }

#endregion


#region Movement

        private void MoveCharacter(Vector3 moveDirection)
        {
            transform.Translate(moveDirection * playerMovementSpeed * Time.deltaTime, Space.World);
            animator.SetBool("IsRunning", true);
        }
        private Vector3 CalculateMoveDirection(){
            return Quaternion.Euler(0, 45, 0) * new Vector3(moveInput.x, 0f, moveInput.y);
        }

        private void RotateCharacter(Quaternion rotation){
            if(new Vector3(lookTarget.x, 0f, lookTarget.z) != Vector3.zero){
                playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, rotation, lookSmoothing);

                if(IsOwner){
                    playerRotation.Value = rotation;
                }
            }
        }
        private Quaternion CalculatePlayerRotation(){
            Ray ray = playerCamera.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit)){
                lookTarget = hit.point;
            }
            var lookPositon = lookTarget - transform.position;
            lookPositon.y = 0;
            return Quaternion.LookRotation(lookPositon);
        }

#endregion



    }
}