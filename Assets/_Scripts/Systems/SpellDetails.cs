using System.Collections.Generic;
using UnityEngine;

public static class SpellDetails
{
    private static Dictionary<int, string> spellDictionary = new Dictionary<int, string>{
        { 1, "Spell 1" },
        { 2, "Spell 2" },
        { 3, "Spell 3" }
    };

    public static void GetSpellDetails(int id){
        if(spellDictionary.TryGetValue(id, out string spellName)){
            Debug.Log($"Found spell: {spellName}");            
        }
        else{
            Debug.LogError($"No spell found with id: {id}");
        }
    }

}