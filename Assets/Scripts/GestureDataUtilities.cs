using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GestureDataUtilities
{
    public static float ScoreGesture_ProgressComp(GestureSample inputGesture, GestureSample templateGesture, string templateName)
    {
        float totalScore = 0f;
        Vector2[] templatePositions = (templateGesture.positions);

        StringBuilder scoreRecord = new();
        
        for (int index = 0; index < inputGesture.positions.Length; index++)
        {
            Vector2 inputPosition = inputGesture.positions[index];

            float progress = index / (inputGesture.positions.Length - 1f);
            Vector2 comparativeTemplatePosition = GetPositionAtProgress(templatePositions, progress);
            float score = Vector2.Distance(comparativeTemplatePosition, inputPosition);
            totalScore += score;

            scoreRecord.Append($"Progress Comp ({templateName}): [{index}] {inputPosition} comparing to progress point {comparativeTemplatePosition} with score {score}. Total score = {totalScore}\n");
        }

        totalScore /= inputGesture.positions.Length;
        Logger.Log($"Final progress ({templateName}) comp score: {totalScore}\n{scoreRecord}", LogType.Scoring);
        
        return totalScore;
    }
    
    public static float ScoreGesture_DistanceComp(GestureSample inputGesture, GestureSample templateGesture, string templateName)
    {
        float totalScore = 0f;
        StringBuilder scoreRecord = new();

        for (int index = 0; index < inputGesture.positions.Length; index++)
        {
            Vector2 inputPosition = inputGesture.positions[index];
            GetDistanceToGesture(inputPosition, templateGesture, out float distanceToGesture, out _);
            totalScore += distanceToGesture;
            
            scoreRecord.Append($"Distance Comp ({templateName}): [{index}] {inputPosition} has distance to gesture of {distanceToGesture}\n");
        }

        totalScore /= inputGesture.positions.Length;
        Logger.Log($"Final distance ({templateName}) comp score: {totalScore}\n{scoreRecord}", LogType.Scoring);
        
        return totalScore;
    }

    public static void GetDistanceToGesture(Vector2 coordinate, GestureSample gesture, out float minGestureDistance, out float atGestureProgress)
    {
        minGestureDistance = float.MaxValue;
        int minDistanceIndex = -1;
        
        Vector2[] gesturePositions = gesture.positions;

        for (int index = 0; index < gesturePositions.Length; index++)
        {
            Vector2 gestureCoordinate = gesturePositions[index];
            float coordinateDistance = Vector2.Distance(coordinate, gestureCoordinate);
            if (coordinateDistance < minGestureDistance)
            {
                minGestureDistance = coordinateDistance;
                minDistanceIndex = index;
            }
        }

        atGestureProgress = minDistanceIndex / (float)gesturePositions.Length;
    }

    private static Vector2 GetPositionAtProgress(Vector2[] positions, float progress)
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

    public static Vector2 ToVector2(this Vector2Int vector2Int)
    {
        return new Vector2(vector2Int.x, vector2Int.y);
    }

    public static Vector2Int ToVector2Int(this Vector2 vector2)
    {
        return new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }

    public static Vector2[] Combine(GestureSample[] gestureSamples)
    {
        int size = gestureSamples.Max(p => p.positions.Length);
        for (int index = 0; index < gestureSamples.Length; index++)
        {
            GestureSample sample = gestureSamples[index];
            gestureSamples[index] = sample.Resample(size);
        }

        Vector2[] results = new Vector2[size];
        for (int i = 0; i < size; i++)
        {
            results[i] = GetAveragedPosition(gestureSamples, i);
        }

        return results.Distinct().ToArray();;
    }

    public static Vector2 GetAveragedPosition(GestureSample[] gestureSamples, int index)
    {
        Vector2 sum = Vector2.zero;
        foreach (GestureSample sample in gestureSamples)
        {
            sum += sample.positions[index];
        }

        sum /= gestureSamples.Length;
        return sum;
    }

    public static Vector2[] Expand(Vector2[] originals, int newSize)
    {
        Vector2[] resampled = new Vector2[newSize];
        
        for (int i = 0; i < newSize; i++)
        {
            float progress = i / (newSize - 1f);
            resampled[i] = GetPositionAtProgress(originals, progress);
        }

        return resampled;
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

    public static Vector2Int[] FillCells(Vector2Int[] quantisedPositions)
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
    
    public static Bounds2D GetBounds(List<Vector2> positions)
    {
        Bounds2D bounds = new()
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

        return bounds;
    }
}