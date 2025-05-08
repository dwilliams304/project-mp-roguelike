using ContradictiveGames.Input;
using FishNet.Object;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("Scriptables")]
        public InputReader inputReader;
        
        [Header("Components")]
        public Rigidbody PlayerRB;
        public Transform PlayerTransform;
        public Animator animator;

        [Header("Movement")]
        public float MoveSpeed;

        [Header("Combat")]
        public Transform FirePoint;

        //Private refs
        private Camera playerCamera;
        private PlayerClassData playerClass;
        private PlayerSettings playerSettings;
        private AttackData mainAttack;
        private AttackData secondaryAttack;

        //Private variables
        private Vector3 lookTarget;
        private Vector3 lookPosition;
        private Vector2 moveInput;

        private bool hasControl = false;


#region Initialization/setup

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if(!IsOwner){
                enabled = false;
                return;
            }

            PlayerManager setup = GetComponent<PlayerManager>();
            if(setup != null){
                playerClass = setup.PlayerClassData;
                playerSettings = setup.PlayerSettings;
                playerCamera = setup.PlayerCamera;
            }
            if(playerClass != null){
                mainAttack = playerClass.MainAttack;
                secondaryAttack = playerClass.SecondaryAttack;
            }

            hasControl = false;

            inputReader.Move += OnMoveInput;
            inputReader.Look += OnLookInput;


            inputReader.MainAttack += OnMainAttack;
            inputReader.SecondaryAttack += OnSecondaryAttack;
            inputReader.Ability1 += OnAbility1Performed;
            inputReader.Ability2 += OnAbility2Performed;
            inputReader.Ability3 += OnAbility3Performed;
            inputReader.Ability4 += OnAbility4Performed;

            inputReader.EnablePlayerActions();
        }


        private void OnDisable()
        {

            inputReader.Move -= OnMoveInput;
            inputReader.Look -= OnLookInput;


            inputReader.MainAttack -= OnMainAttack;
            inputReader.SecondaryAttack -= OnSecondaryAttack;
            inputReader.Ability1 -= OnAbility1Performed;
            inputReader.Ability2 -= OnAbility2Performed;
            inputReader.Ability3 -= OnAbility3Performed;
            inputReader.Ability4 -= OnAbility4Performed;
        }

        public void TogglePlayerMovementControls(bool toggle) => hasControl = toggle;


#endregion


#region Core Loop

        private void Update()
        {
            if(!hasControl) return;
            MovePlayer();
        }

#endregion


#region Movement

        private void OnMoveInput(Vector2 moveInput)
        {
            this.moveInput = moveInput;
        }
        private void MovePlayer()
        {
            //Check
            if (moveInput == Vector2.zero)
            {
                PlayerRB.linearVelocity = Vector3.zero;
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

            PlayerRB.linearVelocity = adjustedMovement;
            //Move the rigidbody

            //Set animator values based on local position
            Vector3 localMove = transform.InverseTransformDirection(moveVector);
            animator.SetFloat("Forward", localMove.z);
            animator.SetFloat("Sideways", localMove.x);

        }
        private void OnLookInput(Vector2 lookInput)
        {
            if(!hasControl) return;
            if (lookInput == Vector2.zero) return;


            Ray ray = playerCamera.ScreenPointToRay(lookInput);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, playerSettings.MouseHitLayer))
            {
                lookTarget = hit.point;
            }
            lookPosition = lookTarget - PlayerTransform.position;
            lookPosition.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPosition);

            // rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, playerSettings.LookSmoothing));
            PlayerTransform.rotation = Quaternion.Slerp(PlayerTransform.rotation, rotation, playerSettings.LookSmoothing);
        }

#endregion


#region Attacking

        private void OnMainAttack()
        {
            HandleAttack(mainAttack);
        }

        private void OnSecondaryAttack()
        {
            HandleAttack(secondaryAttack);
        }

        /// <summary>
        /// Handle the attack logic checks
        /// </summary>
        /// <param name="attack">Attack data needed</param>
        private void HandleAttack(AttackData attack)
        {
            if(!hasControl) return;
            //Early exit
            if (attack == null)
            {
                CustomDebugger.LogWarning($"No attack supplied!", ConsoleLogLevel.PreBuild);
                return;
            }
            if (attack is MeleeAttack)
            {
                DoMeleeAttack(attack as MeleeAttack);
            }
            else if (attack is RangedAttack)
            {
                DoRangedAttack(attack as RangedAttack);
            }
        }

        private void DoMeleeAttack(MeleeAttack attack)
        {
            CustomDebugger.Log("Melee attack not implemented!");
        }

        private void DoRangedAttack(RangedAttack attack)
        {
            Vector3 origin = FirePoint.position;
            Vector3 direction = FirePoint.forward;

            if (attack.IsProjectile)
            {
                CustomDebugger.LogWarning("Projectile attack not implemented!");

            }
            else
            {
                if (Physics.Raycast(origin, direction, out RaycastHit hit, attack.MaxDistance, playerSettings.DamageableLayers))
                {
                    Debug.DrawLine(origin, hit.point, Color.yellow, 1f);
                    if(hit.collider.TryGetComponent<IHealth>(out IHealth health)){
                        health.Damage(5);
                    }
                }
                else
                {
                    // MISS
                    Vector3 endPoint = origin + direction * attack.MaxDistance;
                    Debug.DrawLine(origin, endPoint, Color.red, 1f); // RED on miss
                }
            }
        }

#endregion


#region Abilities

        private void OnAbility1Performed()
        {
            if(!hasControl) return;
            CustomDebugger.Log("Ability {1} not implemented!");
        }
        private void OnAbility2Performed()
        {
            if(!hasControl) return;
            CustomDebugger.Log("Ability {2} not implemented!");
        }
        private void OnAbility3Performed()
        {
            if(!hasControl) return;
            CustomDebugger.Log("Ability {3} not implemented!");
        }
        private void OnAbility4Performed()
        {
            if(!hasControl) return;
            CustomDebugger.Log("Ability {4} not implemented!");
        }



#endregion



    }
}