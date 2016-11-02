using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor {
    SerializedProperty pathProp;
    SerializedProperty speedProp;
    SerializedProperty pingPongProp;
    SerializedProperty activationProp;
    ReorderableList list;

    void OnEnable()
    {
        pathProp = serializedObject.FindProperty("path");
        speedProp = serializedObject.FindProperty("speed");
        pingPongProp = serializedObject.FindProperty("pingPong");
        activationProp = serializedObject.FindProperty("requiresActivation");
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("path"), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) => 
        {
            EditorGUI.LabelField(rect, "Path");
        };
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField( new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
        };
        list.onRemoveCallback = (ReorderableList l) =>
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        };
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        serializedObject.Update();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(speedProp, new GUIContent("Speed", "How fast the platform moves."));
        EditorGUILayout.PropertyField(pingPongProp, new GUIContent("Ping-Pong", "Causes the platform to reverse order\nonce it reaches the end of the path."));
        EditorGUILayout.PropertyField(activationProp, new GUIContent("Requires Activation", "Sets if the platform needs to be activated first."));

        EditorGUILayout.Space();
        list.DoLayoutList();
        
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }
}
