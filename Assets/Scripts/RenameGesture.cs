public class RenameGesture : ButtonBehaviour
{
    protected override void OnClicked()
    {
        NameInputOverlay.Instance.ShowNameRequest((newName) =>
        {
            AppView.Instance.ActiveGesture.gestureName = newName;
        });
    }
}