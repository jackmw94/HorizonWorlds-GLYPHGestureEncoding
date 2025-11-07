public class NewGesture : ButtonBehaviour
{
    protected override void OnClicked()
    {
        CreateNewGesture(true);
    }

    public static void CreateNewGesture(bool canCancel)
    {
        NameInputOverlay.Instance.ShowNameRequest("Enter a name for the new gesture: ", canCancel, (success, newGestureName) =>
        {
            if (!success) return;
            
            Gesture gesture = new(newGestureName);
            gesture.Initialise();
            
            GestureContainer.Instance.gestures.Add(gesture);

            App.Instance.ActiveGesture = gesture;
        });
    }
}