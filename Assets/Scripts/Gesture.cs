using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Gesture
{
    public string gestureName;

    [SerializeField, JsonIgnore] private GestureTexture gestureTexture;
    [SerializeField, JsonIgnore] private List<GestureSample> samples;
    [SerializeField, JsonProperty] private GestureSample combinedSamples;
    
    public Texture2D Texture => gestureTexture.Texture;
    public GestureSample GestureSample => combinedSamples;
    public bool IsValid => samples is { Count: > 0 };

    public Gesture(string gestureName)
    {
        this.gestureName = gestureName;
    }

    public void Initialise()
    {
        gestureTexture ??= new GestureTexture();
        gestureTexture.Initialise();
    }

    public void Populate(GestureSample newSample)
    {
        samples ??= new List<GestureSample>();
        samples.Add(newSample);

        Vector2[] combined = GestureDataUtilities.Combine(samples.ToArray());
        combinedSamples = new GestureSample(combined);
        gestureTexture.Regenerate(combinedSamples);
    }

    public void Clear()
    {
        gestureTexture = new GestureTexture();
        gestureTexture.Initialise();
        
        samples.Clear();
        combinedSamples = new GestureSample(Array.Empty<Vector2>());
    }

    public void InvertY()
    {
        foreach (GestureSample sample in samples)
        {
            sample.InvertY();
        }
        combinedSamples.InvertY();
    }
}