using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GestureCreator : MonoBehaviour
{
    [SerializeField] private int cellsPerDimension = 128;
    [SerializeField] private float falloffDistance = 20f;
    [SerializeField] private Texture2D gestureTexture;
    [SerializeField] private UnityEvent<Texture2D> onCreatedTexture;
    [Space]
    [SerializeField] private string cachedName;

    private Color[] allBlack;

    private void Start()
    {
        allBlack = new Color[cellsPerDimension * cellsPerDimension];
        Array.Fill(allBlack, Color.black);
        
        RegenerateTexture();
        onCreatedTexture?.Invoke(gestureTexture);
    }

    private void RegenerateTexture()
    {
        gestureTexture = new Texture2D(cellsPerDimension, cellsPerDimension)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };
        gestureTexture.SetPixels(allBlack);
        gestureTexture.Apply();
    }

    public void CreateGesture(List<Vector2> positions)
    {
        if (positions.Count == 0)
        {
            Debug.LogError("No positions in list. Exiting early");
            return;
        }

        GetBounds(positions, out Bounds2D bounds);
        Vector2[] normPositions = GestureDataUtilities.GetNormalisedPositions(positions, bounds, false, true);
        Vector2Int[] quantisedPositions = GestureDataUtilities.GetQuantisedPositions(normPositions, cellsPerDimension);
        Vector2Int[] filledPositions = FillCells(quantisedPositions);

        Vector2Int[] finalPositions = filledPositions;

        if (AppView.Instance.ActiveGesture.isValid)
        {
            finalPositions = GestureDataUtilities.Combine(filledPositions, AppView.Instance.ActiveGesture.positions);
        }
        
        RegenerateTexture();
        gestureTexture.SetPixels(GetAllTextureCoordinates().Select(p =>
        {
            GetDistanceToGesture(p, finalPositions, out float distanceToGesture, out float atGestureProgress);
            return Color.Lerp(Color.HSVToRGB(atGestureProgress, 1f, 1f), Color.black, Mathf.Clamp01(distanceToGesture / falloffDistance));
        }).ToArray());
        
        gestureTexture.Apply();

        onCreatedTexture?.Invoke(gestureTexture);
        AppView.Instance.ActiveGesture.Populate(gestureTexture, finalPositions);
    }

    private IEnumerable<Vector2Int> GetAllTextureCoordinates()
    {
        for (int y = 0; y < cellsPerDimension; y++)
        {
            for (int x = 0; x < cellsPerDimension; x++)
            {
                yield return new Vector2Int(x, y);
            }
        }
    }

    private static void GetDistanceToGesture(Vector2Int coordinate, Vector2Int[] gesture, out float minGestureDistance, out float atGestureProgress)
    {
        minGestureDistance = float.MaxValue;
        int minDistanceIndex = -1;

        for (int index = 0; index < gesture.Length; index++)
        {
            Vector2Int gestureCoordinate = gesture[index];
            float coordinateDistance = Vector2Int.Distance(coordinate, gestureCoordinate);
            if (coordinateDistance < minGestureDistance)
            {
                minGestureDistance = coordinateDistance;
                minDistanceIndex = index;
            }
        }

        atGestureProgress = minDistanceIndex / (float)gesture.Length;
    }

    private static Vector2Int[] FillCells(Vector2Int[] quantisedPositions)
    {
        int steps = 10;
        List<Vector2Int> filled = new();
        for (int index = 0; index < quantisedPositions.Length - 1; index++)
        {
            Vector2Int from = quantisedPositions[index];
            Vector2Int to = quantisedPositions[index+1];
            for (int lerpStepId = 0; lerpStepId <= 10; lerpStepId++)
            {
                float lerpVal = lerpStepId / (float)steps;
                int xLerped = Mathf.RoundToInt(Mathf.Lerp(from.x, to.x, lerpVal));
                int yLerped = Mathf.RoundToInt(Mathf.Lerp(from.y, to.y, lerpVal));
                Vector2Int lerpedCoord = new(xLerped, yLerped);
                filled.Add(lerpedCoord);
            }
        }

        return filled.Distinct().ToArray();
    }
    
    

    private void GetBounds(List<Vector2> positions, out Bounds2D bounds)
    {
        bounds = new Bounds2D
        {
            MinX = positions[0].x,
            MinY = positions[0].y,
            MaxX = positions[0].x,
            MaxY = positions[0].y
        };

        foreach (Vector2 position in positions)
        {
            if (position.x < bounds.MinX) bounds.MinX = position.x;
            if (position.y < bounds.MinY) bounds.MinY = position.y;
            if (position.x > bounds.MaxX) bounds.MaxX = position.x;
            if (position.y > bounds.MaxY) bounds.MaxY = position.y;
        }
    }

    
}