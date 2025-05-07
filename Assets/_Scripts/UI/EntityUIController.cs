using ContradictiveGames.Entities;
using ContradictiveGames.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class EntityUIController : MonoBehaviour
    {
        [Header("Health Bar")]
        [SerializeField] private Slider healthBarSlider;
        [SerializeField] private Image healthBarFill;
        
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

        public void SetPlayerColors(bool useEnemyColors){

            if(useEnemyColors){
                healthBarFill.color = GameManager.Instance.gameSettings.EnemyColor;
                nameText.color = GameManager.Instance.gameSettings.EnemyColor;
                levelText.color = Color.red;
            }
            else{
                healthBarFill.color = GameManager.Instance.gameSettings.FriendlyColor;
                nameText.color = GameManager.Instance.gameSettings.FriendlyColor;
                levelText.color = Color.white;
            }



        }
    }
}
