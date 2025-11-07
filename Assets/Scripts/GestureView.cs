using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestureView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private RawImage image;
    [SerializeField] private GameObject selected;
    [SerializeField] private Button button;
    [Space]
    [SerializeField] private Gesture cachedGesture;

    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        AppView.Instance.ActiveGesture = cachedGesture;
    }

    public void Populate(Gesture gesture)
    {
        cachedGesture = gesture;
    }

    private void RefreshData()
    {
        text.text = cachedGesture.gestureName;
        image.texture = cachedGesture.Texture;
        selected.SetActive(cachedGesture == AppView.Instance.ActiveGesture);
    }

    private void Update()
    {
        RefreshData();
    }
}