using UnityEngine;

public enum ConsoleLogLevel
{
    None,
    NecessaryOnly,
    PreBuild,
    InDepth
}

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Custom/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game Settings")]
        [Tooltip("How long the game can be active before it ends automatically. (In seconds)")]
            public float MaxGameTime = 36000;
        [Tooltip("How long the countdown timer will go. (In seconds)")]
        public float CountdownTimer = 10;

        [Header("UI Settings")]
        public bool ShowWorldSpaceUI = true;
        public Color FriendlyColor = Color.blue;
        public Color EnemyColor = Color.red;


        [Header("Development Settings")]
        public ConsoleLogLevel consoleLogLevel = ConsoleLogLevel.NecessaryOnly;
        public bool skipCountdownTimer = false;
    }
}
