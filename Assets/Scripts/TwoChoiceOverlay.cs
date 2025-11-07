using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TwoChoiceOverlay : SingletonMonoBehaviour<TwoChoiceOverlay>
{
    public enum UserChoice
    {
        None,
        Left,
        Right
    }
    
    [SerializeField] private TMP_Text promptLabel;
    [SerializeField] private Button leftButton;
    [SerializeField] private TMP_Text leftButtonText;
    [SerializeField] private Button rightButton;
    [SerializeField] private TMP_Text rightButtonText;
    
    private Action<UserChoice> choiceMadeCallback;
    
    private void OnEnable()
    {
        leftButton.onClick.AddListener(OnLeftChosen);
        rightButton.onClick.AddListener(OnRightChosen);
    }

    private void OnDisable()
    {
        leftButton.onClick.RemoveListener(OnLeftChosen);
        rightButton.onClick.RemoveListener(OnRightChosen);
    }

    public void ShowChoice(string prompt, string leftButtonPrompt, string rightButtonPrompt, Action<UserChoice> callback)
    {
        promptLabel.text = prompt;

        leftButtonText.text = leftButtonPrompt;
        rightButtonText.text = rightButtonPrompt;
        
        choiceMadeCallback = callback;
        gameObject.SetActive(true);
    }

    private void OnLeftChosen()
    {
        choiceMadeCallback.Invoke(UserChoice.Left);
        gameObject.SetActive(false);
    }

    private void OnRightChosen()
    {
        choiceMadeCallback.Invoke(UserChoice.Right);
        gameObject.SetActive(false);
    }
}