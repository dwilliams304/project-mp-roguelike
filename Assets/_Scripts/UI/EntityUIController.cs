using ContradictiveGames.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class EntityUIController : MonoBehaviour
    {
        [Header("Health Bar")]
        [SerializeField] private Slider healthBarSlider;
        
        [Header("Name")]
        [SerializeField] private TMP_Text nameText;
        
        [Header("Level Related")]
        [SerializeField] private Image levelBackground;
        [SerializeField] private Image levelIcon;
        [SerializeField] private TMP_Text levelText;

        public void Initialize(EntityData data){

        }

        public void UpdateHealthBarCurrent(int newAmount){
            healthBarSlider.value = newAmount;
        }
        public void UpdateHealthBarMax(int newMax, bool setCurrentToMax = false){
            healthBarSlider.maxValue = newMax;
            if(setCurrentToMax){
                healthBarSlider.value = newMax;
            }
        }
        public void UpdateEntityName(string name){
            nameText.text = name;
        }

        public void SetAllColors(Color healthbarColor, Color nameColor, Color levelTextColor){
            healthBarSlider.fillRect.GetComponent<Image>().color = healthbarColor;
            nameText.color = nameColor;
            levelText.color = levelTextColor;
        }
    }
}
