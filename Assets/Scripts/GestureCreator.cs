using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestureCreator : MonoBehaviour
{
    [SerializeField] private DrawArea drawArea;

    private void OnEnable()
    {
        drawArea.RegisterListener(this);
    }

    private void OnDisable()
    {
        drawArea.UnregisterListener(this);
    }

    public void CreateGesture(List<Vector2> positions, bool fill = true)
    {
        if (positions.Count == 0)
        {
            Debug.LogError("No positions in list. Exiting early");
            return;
        }

        int cellsPerDimension = GestureConfiguration.Instance.CellsPerDimension;
        
        Bounds2D bounds = GestureDataUtilities.GetBounds(positions);
        Logger.Log($"Bounds = {bounds.MinX}->{bounds.MaxX}, {bounds.MinY}->{bounds.MaxY}", LogType.GestureCreation);
        
        Vector2[] normPositions = GestureDataUtilities.GetNormalisedPositions(positions, bounds, false, true);
        Logger.Log($"normPositions (#{normPositions.Length}): first={normPositions[0]}, last={normPositions[^1]}", LogType.GestureCreation);

        Vector2[] gesturePositions;
        if (fill)
        {
            Vector2Int[] quantisedPositions = GestureDataUtilities.GetQuantisedPositions(normPositions, cellsPerDimension);
            Vector2Int[] filledPositions = GestureDataUtilities.FillCells(quantisedPositions);
            gesturePositions = filledPositions.Select(p => p.ToVector2() / cellsPerDimension).ToArray();
        }
        else
        {
            gesturePositions = normPositions;
        }

        GestureSample gestureSample = new(gesturePositions);

        OnGestureCreated(gestureSample);
    }

    protected virtual void OnGestureCreated(GestureSample gestureSample) { }
    
}