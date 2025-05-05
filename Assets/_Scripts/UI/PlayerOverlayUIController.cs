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


        private void Awake()
        {
            LocalInstance = this;
        }

        
    }
}
