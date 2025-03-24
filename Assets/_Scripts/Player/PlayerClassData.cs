using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [CreateAssetMenu(fileName = "Player Class Data", menuName = "Custom/Player Class Data")]
    public class PlayerClassData : ScriptableObject
    {
        [Tooltip("Name of the class")]
        public string ClassName = "Default Class";
        [Multiline]
        [Tooltip("Class Lore/Description")]
        public string ClassDescription = "Description goes here";

        public Image ClassIcon;
    }
}