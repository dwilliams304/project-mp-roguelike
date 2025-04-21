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
    [CreateAssetMenu(fileName = "New Developer Settings", menuName = "Dev/Developer Settings")]
    public class DeveloperSettings : ScriptableObject
    {
        public ConsoleLogLevel consoleLogLevel = ConsoleLogLevel.NecessaryOnly;
        public bool skipCountdownTimer = false;
    }
}
