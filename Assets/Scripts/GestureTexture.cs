using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GestureTexture
{
    [SerializeField] private byte[] textureBytes;
    [SerializeField] private Texture2D cachedTexture;
    public Texture2D Texture
    {
        get => cachedTexture;
        set
        {
            cachedTexture = value;
            textureBytes = cachedTexture.EncodeToPNG();
        }
    }

    public void Initialise()
    {
        cachedTexture = GetNewTexture();
        if (textureBytes is { Length: > 0 })
        {
            cachedTexture.LoadImage(textureBytes);
        }
    }

    private Texture2D GetNewTexture()
    {
        GestureConfiguration configuration = GestureConfiguration.Instance;
        int cellsPerDimension = configuration.CellsPerDimension;
        
        return new Texture2D(cellsPerDimension, cellsPerDimension)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };
    }

    public void Regenerate(GestureSample gestureSample)
    {
        GestureConfiguration configuration = GestureConfiguration.Instance;

        Texture2D regenTexture = Texture ?? GetNewTexture();
        Color[] textureColours = GetAllTextureCoordinates().Select(p =>
        {
            GestureDataUtilities.GetDistanceToGesture(p, gestureSample, out float distanceToGesture, out float atGestureProgress);
            Color progressColour = Color.HSVToRGB(atGestureProgress, 1f, 1f);
            float colourDistance = Mathf.Clamp01(distanceToGesture / configuration.FalloffDistance);
            return Color.Lerp(progressColour, Color.black, colourDistance);
            
        }).ToArray();
        regenTexture.SetPixels(textureColours);
        regenTexture.Apply();

        Texture = regenTexture;
    }
    
    private IEnumerable<Vector2> GetAllTextureCoordinates()
    {
        GestureConfiguration configuration = GestureConfiguration.Instance;
        int cellsPerDimension = configuration.CellsPerDimension;

        float halfCellOffset = 0.5f / cellsPerDimension;
        
        for (int yIndex = 0; yIndex < cellsPerDimension; yIndex++)
        {
            for (int xIndex = 0; xIndex < cellsPerDimension; xIndex++)
            {
                float xNorm = xIndex / (float)cellsPerDimension;
                float yNorm = yIndex / (float)cellsPerDimension;
                yield return new Vector2(xNorm + halfCellOffset, yNorm + halfCellOffset);
            }
        }
    }
}