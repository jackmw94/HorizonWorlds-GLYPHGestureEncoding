using UnityEngine;
using UnityEngine.UI;

public class ModeMenu : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle[] toggles;

    private void Awake()
    {
        foreach (Toggle toggle in toggles)
        {
            toggleGroup.RegisterToggle(toggle);
        }
    }
}