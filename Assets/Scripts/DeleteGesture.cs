public class DeleteGesture : ButtonBehaviour
{
    protected override void OnClicked()
    {
        GestureContainer.Instance.gestures.Remove(App.Instance.ActiveGesture);
    }
}