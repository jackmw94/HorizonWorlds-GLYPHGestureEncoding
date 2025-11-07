using System;

public class App : SingletonMonoBehaviour<App>
{
    public Gesture ActiveGesture { get; set; } = null;
    
    private void Awake()
    {
        GestureContainer gestureContainer = GestureContainer.Instance;
        gestureContainer.Initialise();
        
        if (gestureContainer.gestures.Count == 0)
        {
            //NewGesture.CreateNewGesture(false);
        }
        else
        {
            ActiveGesture = gestureContainer.gestures[0];
        }
    }
}