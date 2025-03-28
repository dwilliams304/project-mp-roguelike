using System;
using ContradictiveGames.Input;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerStatsHolder))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private InputReader inputReader;
        private PlayerStatsHolder playerStatsHolder;
        private Vector2 moveInput;
        private Vector2 mousePosition;
        private Vector3 lookTarget;

        [SerializeField] private Transform playerModelTransform;


        [SerializeField] private CinemachineCamera cmVCam;
        [SerializeField] private Camera playerCamera;

        //Combat related
        public event Action<AttackSO> PrimaryAttackPerformed;
        public event Action<AttackSO> SecondaryAttackPerformed;

        private AttackSO primaryAttack, secondaryAttack;
        private float lastPrimaryAttack, lastSecondaryAttack;



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


#region Initialization

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
                InitializePlayer();
            }
        }


        private void InitializePlayer(){
            inputReader.Move += MoveDirection => moveInput = MoveDirection;
            inputReader.Look += LookDirection => mousePosition = LookDirection;

            inputReader.MainAttack += DoPrimaryAttack;
            inputReader.SecondaryAttack += DoSecondaryAttack;

            inputReader.EnablePlayerActions();

            playerStatsHolder = GetComponent<PlayerStatsHolder>();
            primaryAttack = playerStatsHolder.playerClassData.PrimaryAttack;
            secondaryAttack = playerStatsHolder.playerClassData.SecondaryAttack;
        }

#endregion


        private void Update()
        {
            if(IsOwner) {
                MoveCharacter(CalculateMoveDirection());
                RotateCharacter(CalculatePlayerRotation());            
            }
            else{
                playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, playerRotation.Value, lookSmoothing);
            }
        }


#region Movement

        private void MoveCharacter(Vector3 moveDirection)
        {
            transform.Translate(moveDirection * playerMovementSpeed * Time.deltaTime, Space.World);
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


#region Combat

        private void DoPrimaryAttack(){
            if(Time.time > primaryAttack.AttackCooldown + lastPrimaryAttack){
                lastPrimaryAttack = Time.time;
                DoAttack(primaryAttack);
                PrimaryAttackPerformed?.Invoke(primaryAttack);
            }
        }
        private void DoSecondaryAttack(){
            if(Time.time > secondaryAttack.AttackCooldown + lastSecondaryAttack){
                lastSecondaryAttack = Time.time;
                DoAttack(secondaryAttack);
                SecondaryAttackPerformed?.Invoke(secondaryAttack);
            }
        }
        private void DoAttack(AttackSO attackSO){
            switch(attackSO.attackType){
                case AttackType.Melee:
                    Debug.Log("Do melee attack!");
                    break;
                case AttackType.Ranged:
                    Debug.Log("Do ranged attack!");
                    break;
            }
        }

#endregion


    }
}