using UnityEngine;
using UnityEngine.UI;

public class TextureUpdater : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private void OnValidate()
    {
        if (!rawImage) rawImage = GetComponent<RawImage>();
    }

    public void SetTexture(Texture2D texture2D)
    {
        rawImage.texture = texture2D;
    }
}