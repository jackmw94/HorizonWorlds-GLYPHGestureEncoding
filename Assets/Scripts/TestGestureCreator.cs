using UnityEngine;

public class TestGestureCreator : GestureCreator
{
    [SerializeField] private TextureUpdater textureUpdater;
    
    protected override void OnGestureCreated(GestureSample gestureSample)
    {
        GestureTexture gestureTexture = new();
        gestureTexture.Regenerate(gestureSample);
        
        textureUpdater.SetTexture(gestureTexture.Texture);
    } 
}