using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct GestureSample
{
    public Vector2Int[] positions;

    public GestureSample(Vector2Int[] positions)
    {
        this.positions = positions;
    }

    public void Resample(int newCount)
    {
        positions = GestureDataUtilities.Expand(positions.Select(GestureDataUtilities.ToVector2).ToArray(), newCount).Select(GestureDataUtilities.ToVector2Int).ToArray();
    }
}