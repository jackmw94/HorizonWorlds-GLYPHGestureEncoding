public class ActiveGestureCreator : GestureCreator
{
    protected override void OnGestureCreated(GestureSample gestureSample)
    {
        App.Instance.ActiveGesture.Populate(gestureSample);
    } 
}