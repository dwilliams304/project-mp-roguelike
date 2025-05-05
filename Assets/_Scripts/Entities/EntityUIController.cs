using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class EntityUIController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private BaseEntity baseEntity;
        [SerializeField] private Slider healthSlider;


        private void Awake(){
            if(baseEntity == null){
                baseEntity = GetComponentInParent<BaseEntity>();
            }
        }

        private void Start(){
            healthSlider.maxValue = baseEntity.MaxHealth.Value;
            healthSlider.value = baseEntity.CurrentHealth.Value; 
        }
        

        private void UpdateMaxHealthUI(int max, int current){
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }

        private void UpdateHealthUI(int current){
            if(baseEntity.MaxHealth.Value == 0) return;

            healthSlider.value = current;
        }
    }
}