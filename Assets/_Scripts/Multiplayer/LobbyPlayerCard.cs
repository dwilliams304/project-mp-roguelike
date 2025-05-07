using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserData
{
    public string Username;
    public string Title;
    public int Level;
    public Sprite LevelIcon;
    public Sprite RankIcon;
    public Sprite Nameplate;
}

namespace ContradictiveGames
{
    public class LobbyPlayerCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text username;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text level;

        [SerializeField] private Image levelIcon;
        [SerializeField] private Image rankIcon;
        [SerializeField] private Image nameplate;


        public void Initialize(UserData userData){
            username.text = userData.Username;
            title.text = userData.Title;
            level.text = $"Level {userData.Level}";

            
        }
    }
}