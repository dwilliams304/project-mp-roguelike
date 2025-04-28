using ContradictiveGames.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class PlayerReadyUI : MonoBehaviour
    {
        private bool isReady = false;

        [SerializeField] private Button readyButton;
        private TMP_Text buttonText;

        private void Start(){
            if(GameManager.Instance != null){
                GameManager.Instance.GameStateChanged += CheckToHideUI;
            }
        }
        private void OnDisable(){
            if(GameManager.Instance != null)
            {
                GameManager.Instance.GameStateChanged -= CheckToHideUI;
            }
        }

        private void CheckToHideUI(GameStateType state){
            if(state == GameStateType.Active){
                readyButton.gameObject.SetActive(false);
            }
            else if(state == GameStateType.Waiting){
                readyButton.gameObject.SetActive(true);
            }
        }

        private void Awake(){
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            buttonText = readyButton.GetComponentInChildren<TMP_Text>();
            UpdateButton();
        }

        private void OnReadyButtonClicked(){
            isReady = !isReady;
            GameManager.Instance.PlayerReadyServerRpc(isReady);
            UpdateButton();
        }

        private void UpdateButton(){
            buttonText.text = !isReady ? "Ready" : "Unready";
        }
    }
}
