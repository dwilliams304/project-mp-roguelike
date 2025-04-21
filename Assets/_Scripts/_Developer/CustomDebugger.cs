using UnityEngine;

public static class CustomDebugger
{


    public static void Log(string message, ConsoleLogLevel logLevel = ConsoleLogLevel.NecessaryOnly){
        if(CheckLogRequirements(logLevel)) Debug.Log(message);
    }
    public static void LogWarning(string message, ConsoleLogLevel logLevel = ConsoleLogLevel.NecessaryOnly){
        if(CheckLogRequirements(logLevel)) Debug.LogWarning(message);
    }
    public static void LogError(string message, ConsoleLogLevel logLevel = ConsoleLogLevel.NecessaryOnly){
        if(CheckLogRequirements(logLevel)) Debug.LogError(message);
    }

    private static bool CheckLogRequirements(ConsoleLogLevel logLevel){
        return true;
    }
}