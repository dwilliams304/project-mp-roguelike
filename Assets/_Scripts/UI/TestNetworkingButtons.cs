using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class TestNetworkingButtons : MonoBehaviour
    {
        [SerializeField] private Button startHost;
        [SerializeField] private Button startClient;

        private void Awake()
        {
            startHost.onClick.AddListener(() => {
                Debug.Log("Starting host!");
                NetworkManager.Singleton.StartHost();
                HidePanel();
            });
            startClient.onClick.AddListener(() => {
                Debug.Log("Starting client!");
                NetworkManager.Singleton.StartClient();
                HidePanel();
            });
        }

        private void HidePanel(){
            gameObject.SetActive(false);
        }
    }
}
