using ContradictiveGames.Input;
using FishNet.Object;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    public class PlayerMovement : NetworkBehaviour
    {
        //Script refs
        private PlayerBase player;
        private PlayerSettings playerSettings;
        private InputReader inputReader;
        
        //Components
        private Animator animator;
        private Rigidbody rb;
        private Camera playerCamera;

        //Private vars.
        private Vector3 lookTarget;
        private Vector3 lookPosition;
        private Vector2 moveInput;


#region Initialization/De-initialization

        /// <summary>
        /// Initialize player actions, input actions, subscribe to events....
        /// </summary>
        public void Initialize(PlayerBase _player)
        {
            player = _player;
            playerSettings = player.PlayerSettings;

            rb = player.PlayerRB;
            playerCamera = player.PlayerCamera;

            inputReader = player.InputReader;
            if (inputReader != null)
            {
                inputReader.Move += OnMove;
                inputReader.Look += RotateCharacter;
            }
        }
        

        // public override void OnNetworkSpawn()
        // {
        //     animator = GetComponentInChildren<Animator>();
        //     enabled = IsOwner;
        // }


        // public override void OnNetworkDespawn()
        // {
        //     DisableInput();
        // }

        /// <summary>
        /// Deinitialize player actions, unsubscribe to events, etc...
        /// </summary>
        private void DisableInput()
        {
            if (inputReader != null)
            {
                inputReader.Move -= OnMove;
                inputReader.Look -= RotateCharacter;
            }
        }

#endregion


#region Core Loop

        private void FixedUpdate()
        {
            
            MoveCharacter();
        }

#endregion


#region Movement


        private void OnMove(Vector2 moveInput){
            this.moveInput = moveInput;
        }


        private void MoveCharacter()
        {
            //Check
            if(moveInput == Vector2.zero) {
                rb.linearVelocity = Vector3.zero;
                animator.SetFloat("Forward", 0f);
                animator.SetFloat("Sideways", 0f);
                return;
            }
            //Get isometric movement
            Vector3 moveVector = Quaternion.Euler(0, 45, 0) * new Vector3(moveInput.x, 0f, moveInput.y);
            moveVector.Normalize();
            float alignment = Vector3.Dot(lookPosition, moveVector);
            float alignment01 = (alignment + 1f) * 0.5f;

            float curvedMultiplier = playerSettings.MovementSlowdownCurve.Evaluate(alignment01);

            Vector3 adjustedMovement = moveVector * playerSettings.MoveSpeed * curvedMultiplier;

            rb.linearVelocity = adjustedMovement;
            //Move the rigidbody
            
            //Set animator values based on local position
            Vector3 localMove = transform.InverseTransformDirection(moveVector);
            animator.SetFloat("Forward", localMove.z);
            animator.SetFloat("Sideways", localMove.x);
        }


        private void RotateCharacter(Vector2 mousePosition)
        {
            //Check
            if(mousePosition == Vector2.zero) return;

            Ray ray = playerCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, playerSettings.MouseHitLayer))
            {
                lookTarget = hit.point;
            }
            lookPosition = lookTarget - transform.position;
            lookPosition.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPosition);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, playerSettings.LookSmoothing));
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lookSmoothing);
        }

#endregion



    }
}