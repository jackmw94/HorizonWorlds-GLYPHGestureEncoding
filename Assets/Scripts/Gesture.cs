using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Gesture
{
    public string gestureName;
    
    public bool isValid = false;

    [SerializeField] private GestureTexture texture;

    [SerializeField] private List<GestureSample> samples;

    public Gesture(string gestureName)
    {
        this.gestureName = gestureName;
    }

    public void Populate(Vector2Int[] gesturePositions)
    {
        Texture = gestureTexture;

        samples ??= new List<GestureSample>();
        samples.Add(new GestureSample(gesturePositions));
        
        finalPositions = GestureDataUtilities.Combine(filledPositions, AppView.Instance.ActiveGesture.positions);

        isValid = true;
    }
}