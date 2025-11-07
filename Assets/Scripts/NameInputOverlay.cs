using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInputOverlay : SingletonMonoBehaviour<NameInputOverlay>
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button okButton;

    private Action<string> nameSubmittedCallback;

    private void OnEnable()
    {
        inputField.onSubmit.AddListener(OnSubmit);
        okButton.onClick.AddListener(OnSubmit);
    }

    private void OnDisable()
    {
        inputField.onSubmit.RemoveListener(OnSubmit);
        okButton.onClick.RemoveListener(OnSubmit);
    }

    public void ShowNameRequest(Action<string> callback)
    {
        nameSubmittedCallback = callback;
        gameObject.SetActive(true);
    }

    private void OnSubmit()
    {
        OnSubmit(inputField.text);
    }

    private void OnSubmit(string nameString)
    {
        nameSubmittedCallback.Invoke(nameString);
        gameObject.SetActive(false);
    }
}