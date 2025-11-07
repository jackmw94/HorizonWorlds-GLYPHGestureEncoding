using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragInputReceiver : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<Vector2> positions;

    [SerializeField] private UnityEvent<List<Vector2>> onReceivedGesture;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        positions.Clear();
        positions.Add(eventData.position);
    }

    // Drag the selected item.
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            positions.Add(eventData.position);
        }
        else
        {
            Debug.LogError("Not dragging");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        positions.Add(eventData.position);
        onReceivedGesture?.Invoke(positions);
        Debug.Log($"Finished drag with {positions.Count} positions");
    }
}