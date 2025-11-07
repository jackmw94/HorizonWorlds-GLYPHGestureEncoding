using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] protected Button button;

    protected void OnValidate()
    {
        if (!button) button = GetComponent<Button>();
    }

    protected void OnEnable()
    {
        button.onClick.AddListener(OnClicked);
    }

    protected void OnDisable()
    {
        button.onClick.RemoveListener(OnClicked);
    }

    protected abstract void OnClicked();
}