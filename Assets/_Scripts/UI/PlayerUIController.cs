using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames.UI
{
    public class PlayerUIController : NetworkBehaviour
    {
        [SerializeField] private GameObject worldSpaceUI;
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMP_Text username;

        [SerializeField] private bool showEvenIfOwner;

        public override void OnNetworkSpawn()
        {
            if(IsOwner && !showEvenIfOwner){
                worldSpaceUI.SetActive(false);
            }
            else{
                worldSpaceUI.SetActive(true);
            }

        }
    }
}