using System;
using ContradictiveGames.Input;
using ContradictiveGames.Systems.Stats;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    public class PlayerMovement : NetworkBehaviour
    {
        private InputReader inputReader;
        private Stat MoveSpeed;

        private Vector2 moveInput;
        private Vector2 mousePosition;
        private Vector3 lookTarget;

        [SerializeField] private Animator animator;
        private Camera playerCamera;

        [Header("Look Rotation Settings")]
        [Range(0, 3)][SerializeField] private float lookSmoothing = 0.15f;
        [SerializeField] private LayerMask mouseHitLayer;

        [Header("Other Settings")]
        [SerializeField] private float interactionRadius;




#region Initialization/De-initialization

        public void InitializeStats(PlayerStats playerStats) {
            MoveSpeed = playerStats.BaseMoveSpeed;
        }

        /// <summary>
        /// Initialize player actions, input actions, subscribe to events....
        /// </summary>
        public void SetUpInput(InputReader _inputReader, Camera _camera)
        {
            playerCamera = _camera;

            inputReader = _inputReader;
            if (inputReader != null)
            {
                inputReader.Move += MoveDirection => moveInput = MoveDirection;
                inputReader.Look += LookDirection => mousePosition = LookDirection;
            }
        }
        

        public override void OnNetworkSpawn()
        {
            enabled = IsOwner;
        }


        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                DisableInput();
            }
        }

        /// <summary>
        /// Deinitialize player actions, unsubscribe to events, etc...
        /// </summary>
        private void DisableInput()
        {
            if (inputReader != null)
            {
                inputReader.Move -= MoveDirection => moveInput = MoveDirection;
                inputReader.Look -= LookDirection => mousePosition = LookDirection;
            }
        }


#endregion


#region Core Loop

        private void Update()
        {
            MoveCharacter(CalculateMoveDirection());
            if(mousePosition != Vector2.zero){
                RotateCharacter(CalculatePlayerRotation());
            }
        }

#endregion


#region Movement

        private void MoveCharacter(Vector3 moveDirection)
        {
            if(moveDirection != Vector3.zero){
                transform.Translate(moveDirection * MoveSpeed.Value * Time.deltaTime, Space.World);
            }
            Vector3 localMove = transform.InverseTransformDirection(moveDirection);
            animator.SetFloat("Forward", localMove.normalized.z);
            animator.SetFloat("Sideways", localMove.normalized.x);
        }
        private Vector3 CalculateMoveDirection()
        {
            return Quaternion.Euler(0, 45, 0) * new Vector3(moveInput.x, 0f, moveInput.y);
        }

        private void RotateCharacter(Quaternion rotation)
        {
            if (new Vector3(lookTarget.x, 0f, lookTarget.z) != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lookSmoothing);
            }
        }
        private Quaternion CalculatePlayerRotation()
        {
            Ray ray = playerCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, mouseHitLayer))
            {
                lookTarget = hit.point;
            }
            var lookPositon = lookTarget - transform.position;
            lookPositon.y = 0;
            return Quaternion.LookRotation(lookPositon);
        }

#endregion



    }
}