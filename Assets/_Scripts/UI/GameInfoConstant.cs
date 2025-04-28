using TMPro;
using UnityEngine;

namespace ContradictiveGames
{
    public class GameInfoConstant : MonoBehaviour
    {
        private static GameInfoConstant instance;
        [SerializeField] private TMP_Text versionText;


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }


            DontDestroyOnLoad(gameObject);

            GetComponent<Canvas>().sortingOrder = 9999;

        }

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
