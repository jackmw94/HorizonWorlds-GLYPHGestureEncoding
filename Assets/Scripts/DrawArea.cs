using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<Vector2> positions;

    private readonly HashSet<GestureCreator> listeners = new();
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        positions.Clear();
        positions.Add(eventData.position);
        Logger.Log($"Adding initial position {eventData.position}", LogType.DrawInputs);
    }

    public void OnDrag(PointerEventData eventData)
    {
        positions.Add(eventData.position);
        
        Logger.Log($"Adding position {eventData.position}", LogType.DrawInputs);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        positions.Add(eventData.position);
        Logger.Log($"Adding final position {eventData.position}. Total {positions.Count} positions", LogType.DrawInputs);
        
        foreach (GestureCreator gestureCreator in listeners)
        {
            gestureCreator.CreateGesture(positions);
        }
    }

    public void RegisterListener(GestureCreator gestureCreator)
    {
        listeners.Add(gestureCreator);
    }

    public void UnregisterListener(GestureCreator gestureCreator)
    {
        listeners.Remove(gestureCreator);
    }
}