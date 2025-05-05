using ContradictiveGames.Input;
using FishNet.Object;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("Setup")]
        public InputReader inputReader;
        public Rigidbody PlayerRB;
        public Transform PlayerTransform;
        public PlayerClassData PlayerClass;
        public PlayerSettings playerSettings;
        public Animator animator;

        [Header("Movement")]
        public float MoveSpeed;

        [Header("Combat")]
        public Transform FirePoint;


        //Private refs


        //Private variables
        private Vector3 lookTarget;
        private Vector3 lookPosition;
        private Vector2 moveInput;

        private LayerMask enemyHitLayers;


#region Initialization/setup

        public override void OnStartClient()
        {
            base.OnStartClient();
            enabled = IsOwner;
        }


        private void Start()
        {

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

        public override void OnStopClient()
        {
            base.OnStopClient();

            inputReader.Move -= OnMoveInput;
            inputReader.Look -= OnLookInput;


            inputReader.MainAttack -= OnMainAttack;
            inputReader.SecondaryAttack -= OnSecondaryAttack;
            inputReader.Ability1 -= OnAbility1Performed;
            inputReader.Ability2 -= OnAbility2Performed;
            inputReader.Ability3 -= OnAbility3Performed;
            inputReader.Ability4 -= OnAbility4Performed;
        }


#endregion


#region Core Loop

        private void Update()
        {
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
            if (lookInput == Vector2.zero) return;


            Ray ray = Camera.main.ScreenPointToRay(lookInput);

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

        /// <summary>
        /// Handle the attack logic checks
        /// </summary>
        /// <param name="attack">Attack data needed</param>
        private void HandleAttack(AttackData attack)
        {
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
            Vector3 direction = FirePoint.up;

            if (attack.IsProjectile)
            {
                CustomDebugger.LogWarning("Projectile attack not implemented!");

            }
            else
            {
                if (Physics.Raycast(origin, direction, out RaycastHit hit, attack.MaxDistance, enemyHitLayers))
                {
                    AttackDebugger.DrawDebugLine(origin, hit.point, Color.yellow);
                    if (hit.collider.TryGetComponent<NetworkObject>(out NetworkObject entity))
                    {
                        Debug.Log("Hit damageable layer!");
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

        }
        private void OnAbility2Performed()
        {

        }
        private void OnAbility3Performed()
        {

        }
        private void OnAbility4Performed()
        {

        }


        private void OnMainAttack()
        {
            CustomDebugger.Log("Doing Main Attack!");
        }
        private void OnSecondaryAttack()
        {
            CustomDebugger.Log("Doing Secondary Attack!");
        }

#endregion



    }
}