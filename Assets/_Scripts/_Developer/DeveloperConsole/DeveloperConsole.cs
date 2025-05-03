using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ContradictiveGames.Dev
{
    [DisallowMultipleComponent]
    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance;


        [Header("Settings")]
        public ConsoleSettings consoleSettings;

        [Header("Console Refs")]
        [SerializeField] private CanvasGroup consolePanel;
        [SerializeField] private Transform logEntriesContainer;
        [SerializeField] private GameObject logEntryPrefab;
        [SerializeField] private TMP_InputField consoleInput;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Scrollbar scrollbar;

        private bool consoleShowing = false;


        private void OnEnable()
        {
            Application.logMessageReceived += HandleUnityLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleUnityLog;

        }

        private void Awake()
        {
            if (consoleSettings == null)
            {
                Debug.LogError("Console settings not assigned in Inspector.", this);
                Destroy(this);
            }
            Init();
            InitializeConsoleSettings();
        }
        
        private void Init()
        {
            if (!Application.isPlaying) {
                Debug.Log("Application not playing, not instantiating Developer Console");
                return;
            }

            // if (transform.parent != null) transform.SetParent(null);

            if (Instance == null)
            {
                Instance = this;
            }
            else if(Instance != this)
            {
                Debug.LogWarning("Found duplicate DeveloperConsole Instance, destroying");
            }
            DontDestroyOnLoad(gameObject);
        }

        public void ToggleDeveloperConsole()
        {
            consoleShowing = !consoleShowing;
            if (consoleShowing)
            {
                consolePanel.blocksRaycasts = true;
                consolePanel.alpha = 1;
            }
            else
            {
                consolePanel.blocksRaycasts = false;
                consolePanel.alpha = 0;
            }
        }

        private void InitializeConsoleSettings()
        {

        }


        private void HandleUnityLog(string message, string stackTrace, LogType logType)
        {
            if (consoleSettings.ShowStackTraceInOutput) AddConsoleLog(message, stackTrace, logType);
            else AddConsoleLog(message, logType);
        }

        private void AddConsoleLog(string message, LogType logType)
        {
            if (logEntriesContainer.childCount >= consoleSettings.MaxConsoleLogs)
            {
                Destroy(logEntriesContainer.GetChild(0).gameObject);
            }

            GameObject log = Instantiate(logEntryPrefab, logEntriesContainer);
            TMP_Text logText = log.GetComponent<TMP_Text>();
            logText.color = GetLogColorByType(logType);
            logText.text = $"[{DateTime.Now.ToString("HH:MM:ss")}]: {message}";

            Canvas.ForceUpdateCanvases();
            if (consoleSettings.AutoScrollToBottom) scrollRect.verticalNormalizedPosition = 0f;
        }
        private void AddConsoleLog(string message, string stackTrace, LogType logType)
        {
            string _message = message + " Stack trace: " + stackTrace;
            AddConsoleLog(_message, logType);
        }

        private Color GetLogColorByType(LogType logType)
        {
            return logType switch
            {
                LogType.Log => consoleSettings.LogNormalColor,
                LogType.Assert => consoleSettings.LogAssertColor,
                LogType.Warning => consoleSettings.LogWarningColor,
                LogType.Error => consoleSettings.LogErrorColor,
                LogType.Exception => consoleSettings.LogExceptionColor,
                _ => consoleSettings.LogNormalColor
            };
        }
    }
}
