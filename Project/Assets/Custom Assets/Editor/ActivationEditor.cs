using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(Activation))]
public class ActivationEditor : Editor {
    SerializedProperty amountProp;
    SerializedProperty useAmntProp;
    ReorderableList list;

    void OnEnable()
    {
        amountProp = serializedObject.FindProperty("amountNeeded");
        useAmntProp = serializedObject.FindProperty("useAmountOfObjects");
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("objects"), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, new GUIContent("Objects to Activate", "This is a list of objects that\nare activated with this script."));
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
        serializedObject.Update();

        EditorGUILayout.Space();
        list.DoLayoutList();

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(useAmntProp, new GUIContent("Use Lsit Amount", "Sets the amount needed to activate to the amount of objects."));
        if (!useAmntProp.boolValue)
        {
            EditorGUILayout.PropertyField(amountProp, new GUIContent("Amount", "How many sprites that are needed to activate this trigger."));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
