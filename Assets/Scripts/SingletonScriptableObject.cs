using UnityEngine;

/// <summary>
/// Assumes that there will be an asset in the Resources folder with the exact same name as its class
/// </summary>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    public static T Instance => instance ? instance : instance = Resources.Load<T>(typeof(T).Name);
    private static T instance;
}