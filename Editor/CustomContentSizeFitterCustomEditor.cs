using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomContentSizeFitter), true)]
[CanEditMultipleObjects]
public class CustomContentSizeFitterEditor : ContentSizeFitterEditor
{
    private SerializedProperty _refreshRate;

    protected override void OnEnable()
    {
        base.OnEnable();
        _refreshRate = serializedObject.FindProperty("_refreshRate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_refreshRate, true);
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}