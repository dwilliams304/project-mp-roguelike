using System;
using ContradictiveGames.Input;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    public class PlayerMovement : NetworkBehaviour
    {
        private InputReader inputReader;
        private PlayerClassData playerClassData;

        private Vector2 moveInput;
        private Vector2 mousePosition;
        private Vector3 lookTarget;

        private Animator animator;
        private Transform playerModelTransform;
        private Camera playerCamera;

        [Header("Look Rotation Settings")]
        [Range(0, 3)] [SerializeField] private float lookSmoothing = 0.15f;

        [Header("Other Settings")]
        [SerializeField] private float interactionRadius;

        private NetworkVariable<Quaternion> playerRotation = new NetworkVariable<Quaternion>(
            Quaternion.identity,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner
        );




#region Initialization/De-initialization

        /// <summary>
        /// Initialize player actions, input actions, subscribe to events....
        /// </summary>
        public void Initialize(InputReader _inputReader, PlayerClassData _playerClassData, Transform _playerModelTransform){
            animator = GetComponentInChildren<Animator>();
            playerModelTransform = _playerModelTransform;
            playerCamera = Camera.main;

            inputReader = _inputReader;
            if(inputReader != null){
                inputReader.Move += MoveDirection => moveInput = MoveDirection;
                inputReader.Look += LookDirection => mousePosition = LookDirection;
            }


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
            if(inputReader != null){
                inputReader.Move -= MoveDirection => moveInput = MoveDirection;
                inputReader.Look -= LookDirection => mousePosition = LookDirection;
            }
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
            transform.Translate(moveDirection * 4f * Time.deltaTime, Space.World);
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