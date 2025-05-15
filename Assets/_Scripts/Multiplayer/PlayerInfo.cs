using Steamworks;
using UnityEngine;

namespace ContradictiveGames
{
    public class PlayerInfo
    {
        //Meta
        public int ClientID;
        public SteamId SteamID;

        //Display info
        public string Username;
        public string Title;
        public Sprite Nameplate;
        public Sprite RankIcon;
        public int Level;
    }
}
