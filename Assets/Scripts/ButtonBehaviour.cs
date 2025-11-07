using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Button button;

    protected void OnValidate()
    {
        if (!button) button = GetComponent<Button>();
    }
}