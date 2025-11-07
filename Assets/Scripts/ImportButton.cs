using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using SFB;
using UnityEngine;

public class ImportButton : ButtonBehaviour
{
    protected override void OnClicked()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Import Gesture Data", "", "json", false);
        if (paths.Length == 0)
        {
            Debug.LogError("Cancelled import");
            return;
        }

        string path = paths[0];
        
        string serializedData = File.ReadAllText(path);
        JObject data = JObject.Parse(serializedData);
        JArray gestures = (JArray)data["gestures"];
        foreach (JToken gestureToken in gestures)
        {
            JToken gestureNameToken = gestureToken["gestureName"];
            JToken gestureSampleToken = gestureToken["combinedSamples"];
            JArray allPositionsToken = (JArray)gestureSampleToken["positions"];

            Vector2[] positions = new Vector2[allPositionsToken.Count];
            for (int index = 0; index < allPositionsToken.Count; index++)
            {
                JToken positionToken = allPositionsToken[index];
                JToken xToken = positionToken["x"];
                JToken yToken = positionToken["y"];

                float x = xToken.Value<float>();
                float y = yToken.Value<float>();

                positions[index] = new Vector2(x, y);
            }

            string gestureName = gestureNameToken.Value<string>();
            
            Gesture gestureInstance = new(gestureName);
            gestureInstance.Initialise();
            gestureInstance.Populate(new GestureSample(positions));
            
            GestureContainer.Instance.gestures.Add(gestureInstance);
        }
    }
}