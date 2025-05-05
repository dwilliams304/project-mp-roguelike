using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class EntityUIController : MonoBehaviour
    {
        [Header("Health Bar")]
        [SerializeField] private Slider HealthBarSlider;
        
        [Header("Name")]
        [SerializeField] private TMP_Text NameText;
        
        [Header("Level Related")]
        [SerializeField] private Sprite levelBackground;
        [SerializeField] private Sprite levelIcon;
        [SerializeField] private TMP_Text levelText;

        public void SetHealthBarMax(int newMax, int oldMax, bool asServer){
            HealthBarSlider.maxValue = newMax;
        }
        public void SetHealthBarMax(int newMax, int oldMax, bool asServer, bool setCurrent = false){
            if(setCurrent){
                SetHealthBarMax(newMax, oldMax, asServer);
                SetHealthBarCurrent(newMax, oldMax, asServer);
            }
            else{
                SetHealthBarMax(newMax, oldMax, asServer);
            }
        }
        public void SetHealthBarCurrent(int newValue, int oldValue, bool asServer){
            HealthBarSlider.value = newValue;
        }

        public void SetAllColors(Color healthbarColor, Color nameColor, Color levelTextColor){
            HealthBarSlider.fillRect.GetComponent<Image>().color = healthbarColor;
            NameText.color = nameColor;
            levelText.color = levelTextColor;
        }
    }
}
