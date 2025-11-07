using UnityEngine;

public class DeleteGesture : ButtonBehaviour
{
    [SerializeField] private bool deleteAllGestures;
    
    protected override void OnClicked()
    {
        if (GestureContainer.Instance.gestures.Count == 0)
        {
            Debug.LogError("No gestures to delete. Returning early");
            return;
        }
        
        if (deleteAllGestures)
        {
            CheckDeleteAll();
        }
        else
        {
            CheckDeleteCurrent();
        }
    }

    private void CheckDeleteCurrent()
    {
        TwoChoiceOverlay.Instance.ShowChoice($"Are you sure you want to delete gestures '{App.Instance.ActiveGesture.gestureName}'?", "Cancel", "Delete", choice =>
        {
            if (choice != TwoChoiceOverlay.UserChoice.Right) return;
            
            // deleting single
            GestureContainer gestureContainer = GestureContainer.Instance;
            gestureContainer.gestures.Remove(App.Instance.ActiveGesture);

            //NewGesture.CreateNewGesture(false);
            App.Instance.ActiveGesture = gestureContainer.gestures.Count == 0 ? null : gestureContainer.gestures[0];
        });
        
    }

    private void CheckDeleteAll()
    {
        TwoChoiceOverlay.Instance.ShowChoice("Are you sure you want to delete all gestures?", "Cancel", "Delete All", choice =>
        {
            if (choice != TwoChoiceOverlay.UserChoice.Right) return;
            
            // deleting all
            GestureContainer gestureContainer = GestureContainer.Instance;
            gestureContainer.gestures.Clear();

            App.Instance.ActiveGesture = null;

            //NewGesture.CreateNewGesture(false);
        });
    }

    private void Update()
    {
        button.interactable = GestureContainer.Instance.gestures.Count > 0;
    }
}