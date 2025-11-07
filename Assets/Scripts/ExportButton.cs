using System.IO;
using SFB;
using UnityEngine;

public class ExportButton : ButtonBehaviour
{
    protected override void OnClicked()
    {
        string serializedData = GestureContainer.Instance.Serialize();
        
        string path = StandaloneFileBrowser.SaveFilePanel("Save Gesture Data", "", "", "json");
        if (path.Length == 0)
        {
            Debug.LogError("Cancelled save");
            return;
        }
        
        File.WriteAllText(path, serializedData);
    }
}