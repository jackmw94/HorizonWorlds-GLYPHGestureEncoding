using System.Collections.Generic;
using UnityEngine;

public class GestureList : MonoBehaviour
{
    [SerializeField] private GameObject gestureViewPrefab;
    [SerializeField] private Transform listRoot;
    [Space]
    [SerializeField] private GestureContainer gestureContainer;

    private readonly Dictionary<Gesture, GestureView> gestureViews = new();

    private void Awake()
    {
        RefreshGestures();
    }

    private void Update()
    {
        if (gestureContainer.gestures.Count != gestureViews.Count)
        {
            RefreshGestures();
        }
    }

    private void RefreshGestures()
    {
        gestureViews.Clear();
        for (int childIndex = listRoot.childCount - 1; childIndex >= 0; childIndex--)
        {
            Transform child = listRoot.GetChild(childIndex);
            Destroy(child.gameObject);
        }
        
        foreach (Gesture gesture in gestureContainer.gestures)
        {
            GestureView gestureView = CreateViewForGesture(gesture);
            gestureViews.Add(gesture, gestureView);
        }
    }

    private GestureView CreateViewForGesture(Gesture gesture)
    {
        GameObject instance = Instantiate(gestureViewPrefab, listRoot);
        GestureView view = instance.GetComponent<GestureView>();
        view.Populate(gesture);

        return view;
    }
}