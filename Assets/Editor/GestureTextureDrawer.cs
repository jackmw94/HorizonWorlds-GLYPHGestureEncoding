using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GestureTexture))]
public class GestureTextureDrawer : PropertyDrawer
{
    private const float PreviewSize = 64f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return PreviewSize + EditorGUIUtility.standardVerticalSpacing * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty textureProp = property.FindPropertyRelative("cachedTexture");
        if (textureProp == null)
        {
            EditorGUI.LabelField(position, label.text, "No texture field found");
            EditorGUI.EndProperty();
            return;
        }

        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        EditorGUI.PrefixLabel(labelRect, label);

        Rect previewRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, PreviewSize, PreviewSize);

        // Draw texture preview
        Texture2D texture = textureProp.objectReferenceValue as Texture2D;
        if (texture != null)
        {
            EditorGUI.DrawPreviewTexture(previewRect, texture, null, ScaleMode.ScaleToFit);
        }
        else
        {
            EditorGUI.HelpBox(previewRect, "No texture", MessageType.None);
        }


        EditorGUI.EndProperty();
    }
}
