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
        //Scriptable objects
        [SerializeField] private PlayerClassData playerClassData;
        [SerializeField] private InputReader inputReader;

        //Object refs
        [SerializeField] private GameObject cameraSetup;
        [SerializeField] private Transform firePoint;

        //Private object refs
        private Camera mainCamera;
        private CinemachineCamera virtualCam;

        //Script refs
        private PlayerStats playerStats;
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        private void Start()
        {
            if (IsOwner)
            {
                Initialize();
            }

            if (playerClassData == null) CustomDebugger.LogError("No Player Class Data has been assigned!");
            if (inputReader == null) CustomDebugger.LogError("No Input Reader has been assigned!");

            if (cameraSetup == null) CustomDebugger.LogError("No Camera Setup has been assigned!");

            if (mainCamera == null) CustomDebugger.LogError("No Main Camera has been assigned!");
            if (virtualCam == null) CustomDebugger.LogError("No Virtual Camera has been assigned!");
        }

        public override void OnNetworkSpawn()
        {
            cameraSetup = Instantiate(cameraSetup);
            cameraSetup.name = $"{gameObject.name} Camera Setup";
            virtualCam = cameraSetup.GetComponentInChildren<CinemachineCamera>();
            mainCamera = cameraSetup.GetComponentInChildren<Camera>();

            // playerStats = GetComponent<PlayerStats>();
            playerStats = new PlayerStats(playerClassData);
            // playerStats.InitializeStats(playerClassData);

            playerMovement = GetComponent<PlayerMovement>();
            playerCombat = GetComponent<PlayerCombat>();
            
            playerMovement.InitializeStats(playerStats);
            playerCombat.InitializeStats(playerStats);

            if(IsOwner){
                playerMovement.SetUpInput(inputReader, mainCamera);
                playerCombat.SetUpInput(inputReader, firePoint);
                Initialize();
            }
            else{
                DeInitialize();
            }
        }


        public override void OnDestroy()
        {
            Destroy(cameraSetup);
        }


        // Initialize self
        private void Initialize()
        {

            virtualCam.enabled = true;
            mainCamera.enabled = true;

            virtualCam.Priority = 100;
            virtualCam.Follow = transform;
            virtualCam.LookAt = transform;

            inputReader.EnablePlayerActions();

        }

        //De-initialize self
        private void DeInitialize()
        {
            virtualCam.enabled = false;
            mainCamera.enabled = false;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            // Destroy(cameraSetup);
        }
    }
}
