using ContradictiveGames.Managers;
using ContradictiveGames.Player;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class PlayerWorldspaceUIController : MonoBehaviour
    {
        private Canvas worldSpaceCanvas;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text usernameText;

        [SerializeField] private Image healthBarFill;

        private PlayerBase player;

        private void Start(){
            player = GetComponentInParent<PlayerBase>();

            if(!GameManager.Instance.gameSettings.ShowWorldSpaceUI && player.IsOwner ) {
                gameObject.SetActive(false);
            }

            worldSpaceCanvas = GetComponent<Canvas>();
            healthBarFill = healthSlider.fillRect.GetComponent<Image>();

            GameManager.Instance.CurrentGameStateType.OnChange += UpdateUI;

            if(GameManager.Instance.CurrentGameStateType.Value == GameStateType.Waiting && !player.IsOwner){
                SetColorUIColors(GameManager.Instance.gameSettings.EnemyColor);
            }
            else{
                SetColorUIColors(GameManager.Instance.gameSettings.FriendlyColor);
            }
            
        }

        private void OnDestroy()
        {
            GameManager.Instance.CurrentGameStateType.OnChange -= UpdateUI;
        }

        private void UpdateUI(GameStateType previousValue, GameStateType newValue, bool asServer)
        {
            if(newValue == GameStateType.Waiting){
                SetColorUIColors(GameManager.Instance.gameSettings.EnemyColor);
            }
            else{
                SetColorUIColors(GameManager.Instance.gameSettings.FriendlyColor);
            }
        }

        private void SetColorUIColors(Color col){
            healthBarFill.color = col;
            usernameText.color = col;
        }

    }
}
