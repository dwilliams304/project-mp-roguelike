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

            if(stats == null) return;
            stats.CurrentHealth.OnChange += UpdateHealth;
            stats.MaxHealth.OnChange += UpdateHealthMax;
            stats.CurrentLevel.OnChange += UpdateCurrentLevel;
            stats.XpToNextLevel.OnChange += UpdateXPToNext;
            stats.CurrentXP.OnChange += UpdateCurrentXP;

            // Manually force UI to correct state after subscription
            UpdateHealth(0, stats.CurrentHealth.Value, true);
            UpdateHealthMax(0, stats.MaxHealth.Value, true);
            UpdateCurrentLevel(0, stats.CurrentLevel.Value, true);
            UpdateXPToNext(0, stats.XpToNextLevel.Value, true);
            UpdateCurrentXP(0, stats.CurrentXP.Value, true);

            UpdateLevelText(stats.CurrentLevel.Value);
        }



        private void OnDestroy()
        {
            if(stats == null) return;
            stats.CurrentHealth.OnChange -= UpdateHealth;
            stats.MaxHealth.OnChange -= UpdateHealthMax;
            stats.CurrentLevel.OnChange -= UpdateCurrentLevel;
            stats.XpToNextLevel.OnChange -= UpdateXPToNext;
            stats.CurrentXP.OnChange -= UpdateCurrentXP;

        }


        private void UpdateHealth(int previousValue, int newValue, bool asServer)
        {
            healthSlider.value = newValue;
        }

        public void UpdateHealthMax(int previousValue, int newValue, bool asServer)
        {
            healthSlider.maxValue = newValue;
            healthSlider.value = stats.CurrentHealth.Value;
        }

        private void UpdateCurrentXP(int previousValue, int newValue, bool asServer)
        {
            xpSlider.value = newValue;
        }

        private void UpdateXPToNext(int previousValue, int newValue, bool asServer)
        {
            xpSlider.maxValue = newValue;
        }

        private void UpdateCurrentLevel(int previousValue, int newValue, bool asServer)
        {
            UpdateLevelText(newValue);
        }

        private void UpdateLevelText(int newLevel)
        {
            levelText.text = $"Lvl. {newLevel}";
        }
    }
}
