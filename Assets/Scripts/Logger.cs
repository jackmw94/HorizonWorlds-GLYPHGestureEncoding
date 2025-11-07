using System;
using UnityEngine;

public enum LogType
{
    GestureCreation,
    DrawInputs,
    Scoring
}

[CreateAssetMenu(menuName = "Create Logger", fileName = "Logger", order = 0)]
public class Logger : SingletonScriptableObject<Logger>
{
    [field: SerializeField] public bool drawInputs = false;
    [field: SerializeField] public bool gestureCreation = false;
    [field: SerializeField] public bool scoring = false;

    public static void Log(string message, LogType logType)
    {
        if (!Instance.IsLogTypeActive(logType)) return;
        
        Debug.Log(message);
    }

    private bool IsLogTypeActive(LogType logType)
    {
        switch (logType)
        {
            case LogType.GestureCreation: return gestureCreation;
            case LogType.DrawInputs: return drawInputs;
            case LogType.Scoring: return scoring;
            default: throw new ArgumentException($"No case for log type: {logType}");
        }
    }
}