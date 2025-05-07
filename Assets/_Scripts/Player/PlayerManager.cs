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

        [Header("Settings")]
        public PlayerClassData PlayerClassData;
        public PlayerSettings PlayerSettings;


        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner)
            {
                if (VirtualCamera == null || PlayerCamera == null)
                {
                    CustomDebugger.LogError("No cameras assigned in inspector!");
                }

                VirtualCamera.Priority = 100;
                PlayerCamera.GetComponent<AudioListener>().enabled = true;

                gameObject.tag = Constants.PlayerSelfTag;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerSelfTag));

                SeeThroughShaderSync sts = GetComponent<SeeThroughShaderSync>();
                sts.enabled = true;
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

        }


        public override void OnStopClient(){
            base.OnStopClient();
            GameManager.Instance.CurrentGameStateType.OnChange -= CheckGameState;
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
                gameObject.tag = Constants.PlayerOtherTag_PVP;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerOtherTag_PVP));
                entityUIController.SetPlayerColors(true);
            }
            else{
                gameObject.tag = Constants.PlayerOtherTag_NoPVP;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerOtherTag_NoPVP));
                entityUIController.SetPlayerColors(false);
            }
        }



    }
}
