public class LiveTextureUpdater : TextureUpdater
{
    private void Update()
    {
        SetTexture(App.Instance.ActiveGesture.Texture);
    }
}