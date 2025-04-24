using ContradictiveGames.Managers;
using UnityEngine;

namespace ContradictiveGames
{
    public class PlayerWaitingUI : MonoBehaviour
    {
        [SerializeField] private GameObject waitingUI;

        private void Start(){
            GameManager.Instance.OnLocalPlayerReady += HideUI;

            waitingUI.SetActive(true);
        }

        private void HideUI(){
            if(GameManager.Instance.IsLocalPlayerReady()){
                waitingUI.SetActive(false);
            }
        }

        public void OnClick(){
            GameManager.Instance.NotifyServerPlayerIsReady();
        }
    }
}
