using ContradictiveGames.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames
{
    public class PlayerWorldspaceUIController : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text usernameText;

        private PlayerManager playerManager;

        private void Start(){

        }

        private void OnDestroy()
        {
            
        }
    }
}
