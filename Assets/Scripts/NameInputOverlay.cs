using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputOverlay : SingletonMonoBehaviour<NameInputOverlay>
{
    [SerializeField] private TMP_Text promptLabel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    private Action<bool,string> nameSubmittedCallback;

    private void OnEnable()
    {
        inputField.onSubmit.AddListener(OnSubmit);
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        
        okButton.onClick.AddListener(OnSubmit);
        cancelButton.onClick.AddListener(OnCancel);
    }

    private void OnDisable()
    {
        inputField.onSubmit.RemoveListener(OnSubmit);
        inputField.onValueChanged.RemoveListener(OnInputValueChanged);
        
        okButton.onClick.RemoveListener(OnSubmit);
        cancelButton.onClick.RemoveListener(OnCancel);
    }

    public void ShowNameRequest(string prompt, bool canCancel, Action<bool,string> callback)
    {
        promptLabel.text = prompt;
        cancelButton.gameObject.SetActive(canCancel);

        inputField.text = "";
        
        nameSubmittedCallback = callback;
        gameObject.SetActive(true);
    }

    private void OnSubmit()
    {
        OnSubmit(inputField.text);
    }

    private void OnSubmit(string nameString)
    {
        nameSubmittedCallback.Invoke(true,nameString);
        gameObject.SetActive(false);
    }

    private void OnCancel()
    {
        nameSubmittedCallback.Invoke(false,"");
        gameObject.SetActive(false);
    }

    private void OnInputValueChanged(string inputValue)
    {
        okButton.interactable = inputValue.Length > 0;
    }
}