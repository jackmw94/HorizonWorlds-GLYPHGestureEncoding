using System;
using UnityEngine;

public class ActiveGestureCreator : GestureCreator
{
    protected override void OnGestureCreated(GestureSample gestureSample)
    {
        if (App.Instance.ActiveGesture == null)
        {
            Debug.LogError("No gesture active so can't populate. Returning early");
            return;
        }
        
        App.Instance.ActiveGesture.Populate(gestureSample);
    } 
}