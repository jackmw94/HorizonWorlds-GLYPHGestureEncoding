using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GestureDataUtilities
{
    public static Vector2 ToVector2(this Vector2Int vector2Int)
    {
        return new Vector2(vector2Int.x, vector2Int.y);
    }

    public static Vector2Int ToVector2Int(this Vector2 vector2)
    {
        return new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }
    
    public static Vector2Int[] Combine(GestureSample[] gestureSamples)
    {
        int size = gestureSamples.Max(p => p.positions.Length);
        

        List<Vector2> list1 = Expand(positions1.Select(p => new Vector2(p.x, p.y)).ToArray(), size);
        List<Vector2> list2 = Expand(positions2.Select(p => new Vector2(p.x, p.y)).ToArray(), size);

        Vector2Int[] results = new Vector2Int[size];
        for (int i = 0; i < size; i++)
        {
            Vector2 combinedPosition = Vector2.Lerp(list1[i], list2[i], 0.5f);
            results[i] = new Vector2Int(Mathf.RoundToInt(combinedPosition.x), Mathf.RoundToInt(combinedPosition.y));
        }

        return results.Distinct().ToArray();;
    }
    public static Vector2Int[] Combine(Vector2Int[] positions1, Vector2Int[] positions2)
    {
        int size = Mathf.Max(positions1.Length, positions2.Length);

        List<Vector2> list1 = Expand(positions1.Select(p => new Vector2(p.x, p.y)).ToArray(), size);
        List<Vector2> list2 = Expand(positions2.Select(p => new Vector2(p.x, p.y)).ToArray(), size);

        Vector2Int[] results = new Vector2Int[size];
        for (int i = 0; i < size; i++)
        {
            Vector2 combinedPosition = Vector2.Lerp(list1[i], list2[i], 0.5f);
            results[i] = new Vector2Int(Mathf.RoundToInt(combinedPosition.x), Mathf.RoundToInt(combinedPosition.y));
        }

        return results.Distinct().ToArray();;
    }
    
    public static List<Vector2> Expand(Vector2[] originals, int newSize)
    {
        List<Vector2> resampled = new();
        
        for (int i = 0; i < newSize; i++)
        {
            float progress = i / (newSize - 1f);
            resampled.Add(GetProgress(originals, progress));
        }

        return resampled;
    }

    private static Vector2 GetProgress(Vector2[] positions, float progress)
    {
        int numPositions = positions.Length;
        
        Debug.Assert(numPositions > 0);
        if (numPositions == 1) return positions[0];

        float arrayProgress = progress * (numPositions - 1f);
        int lowerIndex = Mathf.FloorToInt(arrayProgress);
        int upperIndex = Mathf.CeilToInt(arrayProgress);

        if (lowerIndex == upperIndex) return positions[lowerIndex];
        
        float progressBetweenPoints = arrayProgress - lowerIndex;
        Vector2 lowerValue = positions[lowerIndex];
        Vector2 upperValue = positions[upperIndex];
        return Vector2.Lerp(lowerValue, upperValue, progressBetweenPoints);
    }
    
    public static Vector2Int[] GetQuantisedPositions(Vector2[] normPositions, int cellsPerDimension) {

        return normPositions.Select(normPos => {

            int quantisedX = Mathf.Clamp(Mathf.FloorToInt(normPos.x * cellsPerDimension), 0, cellsPerDimension-1);
            int quantisedY = Mathf.Clamp(Mathf.FloorToInt(normPos.y * cellsPerDimension), 0, cellsPerDimension-1);

            return new Vector2Int(quantisedX, quantisedY);
        }).ToArray();
    }
    
    public static Vector2[] GetNormalisedPositions(List<Vector2> positions, Bounds2D bounds, bool invertY, bool square)
    {
        if (square)
        {
            float dimensionDifference = Mathf.Abs(bounds.XRange - bounds.YRange);
            if (bounds.XRange > bounds.YRange)
            {
                bounds.MinY -= dimensionDifference / 2;
                bounds.MaxY += dimensionDifference / 2;
            }
            else
            {
                bounds.MinX -= dimensionDifference / 2;
                bounds.MaxX += dimensionDifference / 2;
            }
        }

        return positions.Select(pos =>
        {
            float normX = Mathf.InverseLerp(bounds.MinX, bounds.MaxX, pos.x);
            float normY = Mathf.InverseLerp(bounds.MinY, bounds.MaxY, pos.y);

            if (invertY) normY = 1 - normY;

            return new Vector2(normX, normY);
        }).ToArray();
    }
}