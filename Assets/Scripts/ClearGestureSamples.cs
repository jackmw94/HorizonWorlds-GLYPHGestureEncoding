public class ClearGestureSamples : ButtonBehaviour
{
    protected override void OnClicked()
    {
        App.Instance.ActiveGesture.Clear();
    }

    private void Update()
    {
        button.interactable = GestureContainer.Instance.gestures.Count > 0;
    }
}