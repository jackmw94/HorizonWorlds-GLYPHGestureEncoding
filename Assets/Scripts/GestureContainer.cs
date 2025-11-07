using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(menuName = "Create GestureContainer", fileName = "GestureContainer", order = 0)]
public class GestureContainer : SingletonScriptableObject<GestureContainer>
{
    public List<Gesture> gestures;

    public static bool IsEmpty => Instance.gestures.Count == 0;

    public void Initialise()
    {
        foreach (Gesture gesture in gestures)
        {
            gesture.Initialise();
        }
    }

    public string Serialize()
    {
        JsonSerializerSettings settings = new()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = { new Vector2Converter() }
        };
        
        return JsonConvert.SerializeObject(this, settings);
    }

    [ContextMenu(nameof(InvertY))]
    public void InvertY()
    {
        foreach (Gesture gesture in gestures)
        {
            gesture.InvertY();
        }
    }
}