using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class TestGestureCreator : GestureCreator
{
    [SerializeField] private TextureUpdater textureUpdater;
    [SerializeField] private TMP_Text resultsText;

    protected override void OnGestureCreated(GestureSample inputGestureSample)
    {
        GestureTexture gestureTexture = new();
        gestureTexture.Regenerate(inputGestureSample);

        textureUpdater.SetTexture(gestureTexture.Texture);

        Dictionary<Gesture, float> scoresByGesture = new();
        foreach (Gesture testGesture in GestureContainer.Instance.gestures)
        {
            if (!testGesture.IsValid) continue;

            float progressScore = GestureDataUtilities.ScoreGesture_ProgressComp(inputGestureSample, testGesture.GestureSample, testGesture.gestureName);
            float distanceScore = GestureDataUtilities.ScoreGesture_DistanceComp(inputGestureSample, testGesture.GestureSample, testGesture.gestureName);

            float score = progressScore + distanceScore;
            Logger.Log($"Final score for {testGesture.gestureName} = {score}", LogType.Scoring);

            scoresByGesture.Add(testGesture, score);
        }

        StringBuilder sb = new();
        foreach ((Gesture key, float value) in scoresByGesture.OrderBy(p => p.Value))
        {
            sb.Append($"{key.gestureName}: {value:F2}\n");
        }

        resultsText.text = sb.ToString();
    }


    [ContextMenu(nameof(DrawDebugCircle))]
    private void DrawDebugCircle()
    {
        Vector2 center = new(0.5f, 0.5f);
        float radius = 0.25f;
        int pointCount = 20;

        List<Vector2> points = new();

        for (int i = 0; i < pointCount; i++)
        {
            float angle = (i / (float)pointCount) * Mathf.PI * 2 + Mathf.PI / 2;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector2 point = center + (new Vector2(x, y));
            points.Add(point);
        }

        CreateGesture(points, false);
    }

    [ContextMenu(nameof(DrawDebugTriangle))]
    private void DrawDebugTriangle()
    {
        Vector2 startPoint = new(0.5f, .7f);
        Vector2 secondPoint = new(0f, 0f);
        Vector2 thirdPoint = new(1f, 0f);

        List<Vector2> points = DrawPoints(true, 3, startPoint, secondPoint, thirdPoint);

        CreateGesture(points, false);
    }

    private List<Vector2> DrawPoints(bool loop, int stepCount, params Vector2[] vertices)
    {
        List<Vector2> points = new();
        
        for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
        {
            if (vertexIndex == vertices.Length - 1 && !loop)
            {
                break;
            }

            Vector2 firstPoint = vertices[vertexIndex];
            Vector2 nextPoint = vertices[(vertexIndex + 1) % vertices.Length];
            points.Add(firstPoint);

            for (int stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                float progress = (stepIndex + 1f) / (stepCount + 1f);
                points.Add(Vector2.Lerp(firstPoint, nextPoint, progress));
            }
        }
        
        return points;
    }
}