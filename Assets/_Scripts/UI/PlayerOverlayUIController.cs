using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class PlayerOverlayUIController : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TMP_Text levelText;

        private void Start(){

        }

        private void OnDestroy()
        {
            
        }
    }
}
