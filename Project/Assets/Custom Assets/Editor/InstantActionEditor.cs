using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(InstantAction))]
public class InstantActionEditor : Editor
{
    ReorderableList list;

    void OnEnable()
    {
        //Objects List
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("objs"), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, new GUIContent("Objects to Activate", "This is a list of objects that\nare activated with this script,\nand what value to send them."));
        };
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("obj"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 200, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("value"), GUIContent.none);
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
        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }
}
