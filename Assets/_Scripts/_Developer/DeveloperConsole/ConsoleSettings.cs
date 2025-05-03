using UnityEngine;

namespace ContradictiveGames.Dev
{
    [CreateAssetMenu(fileName = "Console Settings", menuName = "Contradictive/Developer Console/Console Settings")]
    public class ConsoleSettings : ScriptableObject
    {
        [Header("Main Panel")]
        public Color BackColor = Color.black;
        public Color DragBarColor = Color.white;

        [Header("Scrollbar")]
        public Color ScrollBarHandleColor = Color.white;
        public Color ScrollBarBackgroundColor = Color.gray;

        [Header("Input Field")]
        public Color InputFieldColor = Color.black;
        public bool ShowInputFieldCursosr;
        public float InputCursorThickness;
        public Color InputCursorColor = Color.gray;
        public Color InputTextColor = Color.white;
        
        [Header("Console Output")]
        public int MaxConsoleLogs = 200;
        public Color LogErrorColor = Color.red;
        public Color LogAssertColor = Color.green;
        public Color LogExceptionColor = Color.red;
        public Color LogWarningColor = Color.yellow;
        public Color LogNormalColor = Color.white;

        [Header("General Options")]
        private KeyCode ButtonToOpen;
        public bool ShowStackTraceInOutput;
        public bool AutoScrollToBottom;
    }
}