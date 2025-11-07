using System;
using UnityEngine;
using UnityEngine.UI;

public class ModeMenu : MonoBehaviour
{
    [SerializeField] private Toggle creationToggle;
    [SerializeField] private GameObject creationObject;
    [SerializeField] private Toggle testToggle;
    [SerializeField] private GameObject testObject;
    
    private void Awake()
    {
        creationToggle.onValueChanged.AddListener(isOn => creationObject.SetActive(isOn));
        testToggle.onValueChanged.AddListener(isOn => testObject.SetActive(isOn));
        
        creationObject.SetActive(creationToggle.isOn);
        testObject.SetActive(testToggle.isOn);
    }
}