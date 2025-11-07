public class RenameGesture : ButtonBehaviour
{
    protected override void OnClicked()
    {
        App app = App.Instance;
        Gesture activeGesture = app.ActiveGesture;
        
        NameInputOverlay.Instance.ShowNameRequest($"Enter new name for the existing gesture ('{activeGesture.gestureName}'): ", true, (success, newName) =>
        {
            if (success) activeGesture.gestureName = newName;
        });
    }

    private void Update()
    {
        button.interactable = GestureContainer.Instance.gestures.Count > 0;
    }
}