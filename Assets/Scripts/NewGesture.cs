using UnityEngine;
using UnityEngine.UI;

public class NewGesture : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        CreateNewGesture();
    }

    public static void CreateNewGesture()
    {
        NameInputOverlay.Instance.ShowNameRequest(newGestureName =>
        {
            Gesture gesture = new(newGestureName);
            GestureContainer.Instance.gestures.Add(gesture);

            AppView.Instance.ActiveGesture = gesture;
        });
    }
}