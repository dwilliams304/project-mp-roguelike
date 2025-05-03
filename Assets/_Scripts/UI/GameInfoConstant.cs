using TMPro;
using UnityEngine;

namespace ContradictiveGames
{
    public class GameInfoConstant : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionText;

        private void Start()
        {
            UpdateUIInfo();
        }

        private void UpdateUIInfo()
        {
            versionText.text = Application.version.ToString();
        }



    }
}
