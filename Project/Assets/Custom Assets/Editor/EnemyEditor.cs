using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    //Type
    SerializedProperty typeProp;
    //Multiple
    SerializedProperty speedProp;
    //Angry
    SerializedProperty dropProp;
    SerializedProperty heightProp;
    //Snoober
    SerializedProperty pingPongProp;
    ReorderableList list;

    void OnEnable()
    {
        speedProp = serializedObject.FindProperty("speed");
        pingPongProp = serializedObject.FindProperty("pingPong");
        typeProp = serializedObject.FindProperty("type");
        dropProp = serializedObject.FindProperty("dropTime");
        heightProp = serializedObject.FindProperty("dropHeight");
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("path"), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Path");
        };
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
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
        EditorGUILayout.PropertyField(typeProp, new GUIContent("Type", "The type of enemy."));

        EditorGUILayout.Separator();
        switch(typeProp.enumValueIndex)
        {
            default:
            case 0:
                break;
            case 1:
                EditorGUILayout.PropertyField(speedProp, new GUIContent("Speed", "How fast the Snoober Noodle moves."));
                EditorGUILayout.PropertyField(pingPongProp, new GUIContent("Ping-Pong", "Causes the platform to reverse order\nonce it reaches the end of the path."));

                EditorGUILayout.Space();
                list.DoLayoutList();
                break;
            case 2:
                EditorGUILayout.PropertyField(speedProp, new GUIContent("Speed", "How fast the Angry Turtle sinks."));
                EditorGUILayout.PropertyField(dropProp, new GUIContent("Sink Time", "How long before it sinks and how long it stays sunk for."));
                EditorGUILayout.PropertyField(heightProp, new GUIContent("Sink Height", "How far the Angry Turtle sinks."));
                break;
        }

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }
}
