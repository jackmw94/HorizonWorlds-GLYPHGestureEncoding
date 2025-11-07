using System;
using TMPro;
using UnityEngine;

public class HintText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private string hasSelectionMessage = "Draw Here";
    [SerializeField] private string noSelectionMessage = "Create a New Gesture\n\n\n-->";

    private bool hadSelectedGestureLastFrame = false;

    private void Awake()
    {
        UpdateText();
    }

    private void Update()
    {
        bool hasSelectedGesture = !GestureContainer.IsEmpty;
        if (hadSelectedGestureLastFrame != hasSelectedGesture)
        {
            UpdateText();
            hadSelectedGestureLastFrame = hasSelectedGesture;
        }
    }

    private void UpdateText()
    {
        bool hasSelectedGesture = GestureContainer.IsEmpty;
        text.text = hasSelectedGesture ? hasSelectionMessage : noSelectionMessage;
    }
}