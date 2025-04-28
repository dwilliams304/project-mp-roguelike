using ContradictiveGames.Input;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerMovement), typeof(CombatController))]
    public class PlayerManager : NetworkBehaviour
    {
        [Header("Setup")]
        public InputReader InputReader;
        public GameObject CamerasPrefab;
        public Transform FirePoint;

        [Header("Settings")]
        public PlayerClassData PlayerClassData;
        public PlayerSettings PlayerSettings;

        //Components
        [HideInInspector] public Rigidbody PlayerRB;
        [HideInInspector] public Camera PlayerCamera;

        //Private refs
        private CinemachineCamera virtualCam;
        private PlayerMovement playerMovement;
        private CombatController combatController;


        private void Start()
        {
            if (IsOwner)
            {
                Initialize();
            }

            if (PlayerClassData == null) CustomDebugger.LogError("No Player Class Data has been assigned!");
            if (InputReader == null) CustomDebugger.LogError("No Input Reader has been assigned!");

            if (CamerasPrefab == null) CustomDebugger.LogError("No Camera Setup has been assigned!");

            if (PlayerCamera == null) CustomDebugger.LogError("No Main Camera has been assigned!");
            if (virtualCam == null) CustomDebugger.LogError("No Virtual Camera has been assigned!");
        }

        public override void OnNetworkSpawn()
        {
            PlayerRB = GetComponent<Rigidbody>();
            CamerasPrefab = Instantiate(CamerasPrefab);
            CamerasPrefab.name = $"{gameObject.name} Camera Setup";
            virtualCam = CamerasPrefab.GetComponentInChildren<CinemachineCamera>();
            PlayerCamera = CamerasPrefab.GetComponentInChildren<Camera>();

            // playerStats = GetComponent<PlayerStats>();

            playerMovement = GetComponent<PlayerMovement>();
            combatController = GetComponent<CombatController>();
            
            combatController.InitializeCombatController(PlayerClassData);

            Initialize();

            if(IsOwner){
                playerMovement.Initialize(this);
                combatController.Initialize(this);
            }
        }


        public override void OnDestroy()
        {
            if(CamerasPrefab != null) Destroy(CamerasPrefab);
        }


        // Initialize self
        private void Initialize()
        {
            if(IsOwner){
                virtualCam.enabled = true;
                PlayerCamera.enabled = true;

                virtualCam.Priority = 100;
                virtualCam.Follow = transform;
                virtualCam.LookAt = transform;

            }
            else
            {
                virtualCam.Priority = -1;
                virtualCam.enabled = false;
                PlayerCamera.enabled = false;
                PlayerCamera.GetComponent<AudioListener>().enabled = false;
            }

            InputReader.EnablePlayerActions();

        }

    }
}
