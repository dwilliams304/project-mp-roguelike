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
        public const string WorldSpaceCameraName = "WorldSpaceCam";

        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text usernameText;

        [SerializeField] private Image healthBarFill;

        private PlayerManager playerManager;
        private PlayerStats playerStats;

        private void Start(){
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = playerManager.PlayerStats;

            if(!GameManager.Instance.gameSettings.ShowWorldSpaceUI && playerManager.IsOwner ) {
                gameObject.SetActive(false);
            }
            
            worldSpaceCanvas = GetComponent<Canvas>();
            Camera worldSpaceCam = GameObject.Find(WorldSpaceCameraName).GetComponent<Camera>();
            if(worldSpaceCam != null) worldSpaceCanvas.worldCamera = worldSpaceCam;
            healthBarFill = healthSlider.fillRect.GetComponent<Image>();

            playerStats.MaxHealth.OnValueChanged += UpdateHealthBarSliderMax;
            playerStats.CurrentHealth.OnValueChanged += UpdateHealthBar;
            playerManager.Username.OnValueChanged += UpdateUsernameText;
            GameManager.Instance.CurrentGameStateType.OnValueChanged += UpdateUI;

            if(GameManager.Instance.CurrentGameStateType.Value == GameStateType.Waiting && !playerManager.IsOwner){
                SetColorUIColors(GameManager.Instance.gameSettings.EnemyColor);
            }
            else{
                SetColorUIColors(GameManager.Instance.gameSettings.FriendlyColor);
            }
            
            InitializeUIValues();
        }

        private void OnDestroy()
        {
            playerStats.MaxHealth.OnValueChanged -= UpdateHealthBarSliderMax;
            playerStats.CurrentHealth.OnValueChanged -= UpdateHealthBar;
            playerManager.Username.OnValueChanged -= UpdateUsernameText;
            GameManager.Instance.CurrentGameStateType.OnValueChanged -= UpdateUI;
        }

        private void UpdateUI(GameStateType previousValue, GameStateType newValue)
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

        private void InitializeUIValues(){
            UpdateHealthBarSliderMax(0, playerStats.MaxHealth.Value);
            usernameText.text = playerManager.Username.Value.ToString();
        }

        private void UpdateUsernameText(FixedString32Bytes oldVal, FixedString32Bytes newValue){
            usernameText.text = newValue.ToString();
        }

        private void UpdateHealthBarSliderMax(int oldValue, int newValue){
            healthSlider.maxValue = newValue;
            healthSlider.value = playerStats.CurrentHealth.Value;
        }

        private void UpdateHealthBar(int oldValue, int newValue){
            healthSlider.value = newValue;
        }
    }
}
