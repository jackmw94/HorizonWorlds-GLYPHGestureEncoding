public class LiveTextureUpdater : TextureUpdater
{
    private void Update()
    {
        if (App.Instance.ActiveGesture != null)
        {
            SetTexture(App.Instance.ActiveGesture.Texture);
        }
    }
}