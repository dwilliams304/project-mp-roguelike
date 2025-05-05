using Unity.Cinemachine;
using UnityEngine;
using FishNet.Object;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    public class PlayerManager : NetworkBehaviour
    {

        [Header("Setup")]
        public Camera PlayerCamera;
        public CinemachineCamera VirtualCamera;

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
                // VirtualCamera.Follow = transform;
                // VirtualCamera.LookAt = transform;
                PlayerCamera.GetComponent<AudioListener>().enabled = true;

                gameObject.tag = Constants.PlayerSelfTag;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerSelfTag));
            }
            else
            {
                PlayerCamera.enabled = false;
                VirtualCamera.Priority = -5;
                VirtualCamera.enabled = false;


                gameObject.tag = Constants.PlayerOtherTag_PVP;
                gameObject.SetLayersRecursive(LayerMask.NameToLayer(Constants.PlayerOtherTag_PVP));
            }

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
