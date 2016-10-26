using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor {
    ReorderableList pathList;
    SerializedProperty pathProp;

    void OnEnable()
    {
        pathProp = serializedObject.FindProperty("path");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        

    }
}
