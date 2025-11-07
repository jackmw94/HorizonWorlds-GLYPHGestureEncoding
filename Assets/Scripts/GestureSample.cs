using System;
using UnityEngine;

[Serializable]
public class GestureSample
{
    [SerializeField] public Vector2[] positions;
    
    public GestureSample(Vector2[] positions)
    {
        this.positions = positions;
    }

    public GestureSample Resample(int newCount)
    {
        Vector2[] expandedPoints = GestureDataUtilities.Expand(positions, newCount);
        Debug.Assert(expandedPoints.Length == newCount);

        return new GestureSample(expandedPoints);
    }

    public void InvertY()
    {
        for (int index = 0; index < positions.Length; index++)
        {
            Vector2 position = positions[index];
            position.y = 1f - position.y;
            positions[index] = position;
        }
    }
}