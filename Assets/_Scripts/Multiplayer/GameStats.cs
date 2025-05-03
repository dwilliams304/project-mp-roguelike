using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Multiplayer
{
    public class GameStats : NetworkBehaviour
    {
        public static GameStats Instance;

        public struct DamageInfo
        {
            public ulong ClientId;
            public int DamageAmount;
            public int WeaponID;
        }


        
    }
}