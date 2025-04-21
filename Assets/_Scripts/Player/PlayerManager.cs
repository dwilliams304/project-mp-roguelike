using ContradictiveGames.Input;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerCombat))]
    public class PlayerManager : NetworkBehaviour
    {
        //Scriptable Objects
        [SerializeField] private PlayerClassData playerClassData;
        [SerializeField] private InputReader inputReader;

        //Object Refs
        [SerializeField] private Transform playerModelTransform;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineCamera virtualCam;
        [SerializeField] private Transform firePoint;

        //Script refs
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        private void Start(){
            if(IsOwner){
                Initialize();
            }
            
            if(playerClassData == null) CustomDebugger.LogError("No Player Class Data has been assigned!");
            if(inputReader == null) CustomDebugger.LogError("No Input Reader has been assigned!");

            if(playerModelTransform == null) CustomDebugger.LogError("No Player Model Transform has been assigned!");
            if(mainCamera == null) CustomDebugger.LogError("No Main Camera has been assigned!");
            if(virtualCam == null) CustomDebugger.LogError("No Virtual Camera has been assigned!");
        }

        public override void OnNetworkSpawn(){
            if(IsOwner){
                //Set up everything
                Initialize();
            }
            else{
                DeInitialize();
            }
        }


        // Initialize self
        private void Initialize(){
            playerMovement = GetComponent<PlayerMovement>();
            playerCombat = GetComponent<PlayerCombat>();
            playerMovement.Initialize(inputReader, playerClassData, playerModelTransform);
            playerCombat.Initialize(inputReader, playerClassData, firePoint);

            virtualCam.enabled = true;
            mainCamera.enabled = true;

            virtualCam.Priority = 100;
            virtualCam.Follow = transform;
            virtualCam.LookAt = transform;

            inputReader.EnablePlayerActions();
        }

        //De-initialize self
        private void DeInitialize(){
            virtualCam.enabled = false;
            mainCamera.enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
        }
    }
}
