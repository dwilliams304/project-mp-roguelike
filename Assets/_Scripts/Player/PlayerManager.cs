using ContradictiveGames.Input;
using ContradictiveGames.Managers;
using ContradictiveGames.Effects;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(
        typeof(PlayerStats), 
        typeof(PlayerMovement), 
        typeof(CombatController)
    )]
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
        [HideInInspector] public PlayerStats PlayerStats;
        [HideInInspector] public Rigidbody PlayerRB;
        [HideInInspector] public Camera PlayerCamera;

        //Private refs
        private CinemachineCamera virtualCam;
        private PlayerMovement playerMovement;
        private CombatController combatController;
        private int uiLayer;

        public NetworkVariable<FixedString32Bytes> Username = new();


        public override void OnNetworkSpawn()
        {
            PlayerStats = GetComponent<PlayerStats>();
            PlayerRB = GetComponent<Rigidbody>();
            CamerasPrefab = Instantiate(CamerasPrefab);
            CamerasPrefab.name = $"{gameObject.name} Camera Setup";
            virtualCam = CamerasPrefab.GetComponentInChildren<CinemachineCamera>();
            PlayerCamera = CamerasPrefab.GetComponentInChildren<Camera>();

            uiLayer = LayerMask.NameToLayer(Constants.WorldSpaceUITag);


            playerMovement = GetComponent<PlayerMovement>();
            combatController = GetComponent<CombatController>();
            PlayerStats.InitializeStats(PlayerClassData);

            combatController.InitializeCombatController(PlayerClassData);

            if(IsServer){
                Username.Value = $"Player {Random.Range(0, 100)}";
            }

            Initialize();

            ErrorChecks();

            SeeThroughShaderSync sts = GetComponent<SeeThroughShaderSync>();
            if(sts != null) sts.enabled = IsOwner;

            if (IsOwner)
            {
                playerMovement.Initialize(this);
                combatController.Initialize(this);
            }
        }

        private void Start()
        {
            GameManager.Instance.GameStateChanged += OnGameActive;
        }

        private void ErrorChecks()
        {
            if (PlayerClassData == null) CustomDebugger.LogError("No Player Class Data has been assigned!");
            if (InputReader == null) CustomDebugger.LogError("No Input Reader has been assigned!");

            if (CamerasPrefab == null) CustomDebugger.LogError("No Camera Setup has been assigned!");

            if (PlayerCamera == null) CustomDebugger.LogError("No Main Camera has been assigned!");
            if (virtualCam == null) CustomDebugger.LogError("No Virtual Camera has been assigned!");
        }


        public override void OnDestroy()
        {
            if (CamerasPrefab != null) Destroy(CamerasPrefab);
            GameManager.Instance.GameStateChanged -= OnGameActive;
        }


        // Initialize self
        private void Initialize()
        {
            if (IsOwner)
            {
                virtualCam.enabled = true;
                PlayerCamera.enabled = true;
                PlayerCamera.tag = "MainCamera";
                

                virtualCam.Priority = 100;
                virtualCam.Follow = transform;
                virtualCam.LookAt = transform;

                gameObject.tag = Constants.PlayerSelfTag;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerSelfTag));

            }
            else
            {
                Destroy(CamerasPrefab);

                gameObject.tag = Constants.PlayerOtherTag_PVP;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerOtherTag_PVP));
            }

            InputReader.EnablePlayerActions();

        }


        private void OnGameActive(GameStateType state)
        {
            if (state == GameStateType.Active)
            {
                if (!IsOwner)
                {
                    // Change this player's tag and layer because they are someone else's view
                    gameObject.tag = Constants.PlayerOtherTag_NoPVP;
                    gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerOtherTag_NoPVP));
                }
            }
        }

    }
}
