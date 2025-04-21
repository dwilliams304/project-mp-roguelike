using System.Collections.Generic;
using ContradictiveGames.Systems.Stats;

namespace ContradictiveGames.Player
{
    public class PlayerStats
    {
        public Stat MaxHealth;
        public Stat BaseMoveSpeed;


        private readonly Dictionary<StatType, Stat> statLookup = new();

        public PlayerStats (PlayerClassData playerClassData){
            MaxHealth = new Stat(playerClassData.MaxHealth);
            BaseMoveSpeed = new Stat(playerClassData.BaseMoveSpeed);


            statLookup = new Dictionary<StatType, Stat>{
                { StatType.MaxHealth, MaxHealth },
                
            };
        }

        public Stat TryGetStat(StatType statType){
            if(statLookup.TryGetValue(statType, out Stat s)) {
                return s;
            }
            else{
                CustomDebugger.LogError($"No stat found of type: {statType}");
                return null;
            }
        }
    }
}