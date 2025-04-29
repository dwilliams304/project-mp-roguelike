using ContradictiveGames.Player;
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

        private PlayerStats stats;

        private void Awake()
        {
            LocalInstance = this;
        }

        public void InitializeUI(PlayerStats _stats)
        {
            stats = _stats;

            stats.CurrentHealth.OnValueChanged += UpdateHealth;
            stats.MaxHealth.OnValueChanged += UpdateHealthMax;
            stats.CurrentLevel.OnValueChanged += UpdateCurrentLevel;
            stats.XpToNextLevel.OnValueChanged += UpdateXPToNext;
            stats.CurrentXP.OnValueChanged += UpdateCurrentXP;

            // Manually force UI to correct state after subscription
            UpdateHealth(0, stats.CurrentHealth.Value);
            UpdateHealthMax(0, stats.MaxHealth.Value);
            UpdateCurrentLevel(0, stats.CurrentLevel.Value);
            UpdateXPToNext(0, stats.XpToNextLevel.Value);
            UpdateCurrentXP(0, stats.CurrentXP.Value);

            UpdateLevelText(stats.CurrentLevel.Value);
        }



        private void OnDestroy()
        {
            stats.CurrentHealth.OnValueChanged -= UpdateHealth;
            stats.MaxHealth.OnValueChanged -= UpdateHealthMax;
            stats.CurrentLevel.OnValueChanged -= UpdateCurrentLevel;
            stats.XpToNextLevel.OnValueChanged -= UpdateXPToNext;
            stats.CurrentXP.OnValueChanged -= UpdateCurrentXP;

        }


        private void UpdateHealth(int previousValue, int newValue)
        {
            healthSlider.value = newValue;
        }

        public void UpdateHealthMax(int previousValue, int newValue)
        {
            healthSlider.maxValue = newValue;
            healthSlider.value = stats.CurrentHealth.Value;
        }

        private void UpdateCurrentXP(int previousValue, int newValue)
        {
            xpSlider.value = newValue;
        }

        private void UpdateXPToNext(int previousValue, int newValue)
        {
            xpSlider.maxValue = newValue;
        }

        private void UpdateCurrentLevel(int previousValue, int newValue)
        {
            UpdateLevelText(newValue);
        }

        private void UpdateLevelText(int newLevel)
        {
            levelText.text = $"Lvl. {newLevel}";
        }
    }
}
