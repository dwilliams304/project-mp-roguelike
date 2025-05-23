using Unity.Cinemachine;
using UnityEngine;
using FishNet.Object;
using ContradictiveGames.Managers;
using GameKit.Dependencies.Utilities;
using ContradictiveGames.Effects;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    public class PlayerManager : NetworkBehaviour
    {

        [Header("Setup")]
        public Camera PlayerCamera;
        public CinemachineCamera VirtualCamera;
        public EntityUIController entityUIController;
        public GameObject PlayerModel;
        public GameObject PlayerWorldspaceCanvas;

        [Header("Settings")]
        public PlayerClassData PlayerClassData;
        public PlayerSettings PlayerSettings;
        public PlayerController PlayerController;

        private CapsuleCollider coll;
        private AudioListener audioListener;
        private SeeThroughShaderSync sts;

        public override void OnStartClient()
        {
            base.OnStartClient();

            coll = GetComponent<CapsuleCollider>();
            audioListener = PlayerCamera.GetComponentInChildren<AudioListener>();
            sts = GetComponent<SeeThroughShaderSync>();
            coll.enabled = false;
            sts.enabled = false;
            audioListener.enabled = false;
            PlayerModel.SetActive(false);
            PlayerWorldspaceCanvas.SetActive(false);

            if (IsOwner)
            {
                gameObject.tag = GameConstants.PlayerSelfTag;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(GameConstants.PlayerSelfTag));
            }
            else
            {
                PlayerCamera.enabled = false;
                VirtualCamera.Priority = -5;
                VirtualCamera.enabled = false;

                TogglePvpState(true);

                // gameObject.tag = Constants.PlayerOtherTag_PVP;
                // gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerOtherTag_PVP));
            }

            
            GameManager.Instance.CurrentGameStateType.OnChange -= CheckGameState;
            GameManager.Instance.CurrentGameStateType.OnChange += CheckGameState;

            SpawnPlayer();

        }


        public override void OnStopClient(){
            base.OnStopClient();

            GameManager.Instance.CurrentGameStateType.OnChange -= CheckGameState;
        }


        [ServerRpc]
        public void SpawnPlayer(){
            PlayerModel.SetActive(true);
            coll.enabled = true;
            PlayerWorldspaceCanvas.SetActive(true);
            if(IsOwner){
                
                PlayerCamera.enabled = true;
                audioListener.enabled = true;
                VirtualCamera.Priority = 100;
                sts.enabled = true;
            }
            ShowPlayer(true);
        }


        [ServerRpc]
        public void DespawnPlayer(){
            PlayerController.TogglePlayerMovementControls(false);
            coll.enabled = false;
            PlayerModel.SetActive(false);
            PlayerWorldspaceCanvas.SetActive(false);
            ShowPlayer(false);
        }

        [ObserversRpc]
        public void ShowPlayer(bool show){
            PlayerController.TogglePlayerMovementControls(show);
            PlayerModel.SetActive(show);
            coll.enabled = show;
            PlayerWorldspaceCanvas.SetActive(show);
        }
        

        private void CheckGameState(GameStateType prevState, GameStateType newState, bool asServer)
        {
            if(!enabled || gameObject.IsDestroyed()) return;
            switch(newState){
                case GameStateType.Waiting:
                    if(!IsOwner){
                        TogglePvpState(true);
                    }
                    break;
                case GameStateType.Active:
                    if(!IsOwner){
                        TogglePvpState(false);
                    }
                    break;
            }
        }


        private void TogglePvpState(bool pvpEnable){
            if(pvpEnable){
                gameObject.tag = GameConstants.PlayerOtherTag_PVP;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(GameConstants.PlayerOtherTag_PVP));
                entityUIController.SetPlayerColors(true);
            }
            else{
                gameObject.tag = GameConstants.PlayerOtherTag_NoPVP;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(GameConstants.PlayerOtherTag_NoPVP));
                entityUIController.SetPlayerColors(false);
            }
        }



    }
}
