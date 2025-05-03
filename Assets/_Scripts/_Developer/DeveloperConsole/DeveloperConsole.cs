using System;
using ContradictiveGames.Input;
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
        [SerializeField] private InputReader inputReader;

        [Header("Console Refs")]
        [SerializeField] private CanvasGroup consolePanel;
        [SerializeField] private Transform logEntriesContainer;
        [SerializeField] private GameObject logEntryPrefab;
        [SerializeField] private TMP_InputField consoleInput;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Scrollbar scrollbar;

        [Header("Console Graphics Refs")]
        [SerializeField] private Image consoleDragbar;
        [SerializeField] private Image consoleBackground;

        private bool consoleShowing = false;


        private void OnEnable()
        {
            Application.logMessageReceived += HandleUnityLog;
            if(inputReader != null){
                inputReader.Debug -= ToggleDeveloperConsole;
                inputReader.Debug += ToggleDeveloperConsole;
            }
        }

        private void OnDisable()
        {
            if(inputReader != null) inputReader.Debug -= ToggleDeveloperConsole;
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

            if (Instance == null) Instance = this;
            else if(Instance != this)
            {
                Debug.LogWarning("Found duplicate DeveloperConsole Instance, destroying");
                Destroy(this);
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
                consolePanel.interactable = true;
            }
            else
            {
                consolePanel.blocksRaycasts = false;
                consolePanel.alpha = 0;
                consolePanel.interactable = false;
            }
        }

        private void InitializeConsoleSettings()
        {
            consolePanel.blocksRaycasts = false;
            consolePanel.alpha = 0;
            consolePanel.interactable = false;

            SetColors();
        }

        private void SetColors(){
            consoleBackground.color = consoleSettings.BackColor;
            consoleDragbar.color = consoleSettings.DragBarColor;
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
