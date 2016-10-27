using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(TorchActivation))]
public class TorchActivationEditor : Editor
{
    SerializedProperty amountProp;
    SerializedProperty useAmntProp;
    SerializedProperty invertProp;
    SerializedProperty onceProp;
    SerializedProperty emitProp;
    SerializedProperty fireProp;
    ReorderableList list;

    void OnEnable()
    {
        amountProp = serializedObject.FindProperty("spritesNeeded");
        useAmntProp = serializedObject.FindProperty("useAmountInObjects");
        invertProp = serializedObject.FindProperty("invert");
        onceProp = serializedObject.FindProperty("useOnce");
        emitProp = serializedObject.FindProperty("emitterParent");
        fireProp = serializedObject.FindProperty("fireEmitter");
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
        EditorGUILayout.LabelField(new GUIContent("Options", "Options for how the torch works."));
        EditorGUILayout.PropertyField(onceProp, new GUIContent("Use Once", "Allows it to activate once only."));
        EditorGUILayout.PropertyField(invertProp, new GUIContent("Invert", "Sets everything to active first."));
        EditorGUILayout.PropertyField(useAmntProp, new GUIContent("Use Lsit Amount", "Sets the amount needed to activate to the amount of objects."));
        if (!useAmntProp.boolValue)
        {
            EditorGUILayout.PropertyField(amountProp, new GUIContent("Amount", "How many sprites that are needed to activate this trigger."));
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(new GUIContent("References", "References needed for the scripts to work."));
        EditorGUILayout.PropertyField(fireProp, new GUIContent("Fire Emitter", "Reference to the \'Short Fire\' prefab."));
        EditorGUILayout.PropertyField(emitProp, new GUIContent("Fire Transform", "Refernce to the Transform the fire is placed."));
        
        serializedObject.ApplyModifiedProperties();
    }
}
