using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class PlayerOverlayUIController : MonoBehaviour
    {
        public static PlayerOverlayUIController LocalInstance;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TMP_Text levelText;


        private void Awake()
        {
            LocalInstance = this;
        }



        public void UpdateCurrentHealth(int newAmount){
            healthSlider.value = newAmount;
        }
        public void UpdateCurrentMax(int newAmount, bool setCurrentToMax){
            healthSlider.maxValue = newAmount;
            if(setCurrentToMax){
                healthSlider.value = newAmount;
            }
        }
        public void UpdateLevelText(int newLevel, int newXpToNextLevel, int currentXp)
        {
            levelText.text = $"Lvl. {newLevel}";
            xpSlider.maxValue = newXpToNextLevel;
            xpSlider.value = currentXp;
        }
        public void UpdateXP(int newAmount){
            xpSlider.value = newAmount;
        }

    }
}
