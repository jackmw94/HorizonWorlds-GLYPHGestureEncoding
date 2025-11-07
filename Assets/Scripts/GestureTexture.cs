using System;
using UnityEngine;

[Serializable]
public class GestureTexture
{
    [SerializeField] private byte[] textureBytes;
    [SerializeField] private int cellsPerDimension = 128;
    
    private Texture2D cachedTexture;
    public Texture2D Texture
    {
        get
        {
            if (cachedTexture) return cachedTexture;

            cachedTexture = new Texture2D(cellsPerDimension, cellsPerDimension);
            cachedTexture.LoadImage(textureBytes);
            return cachedTexture;
        }
        set
        {
            cachedTexture = value;
            textureBytes = cachedTexture.EncodeToPNG();
        }
    }
}