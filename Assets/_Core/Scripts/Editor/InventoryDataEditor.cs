using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryData))]
public class InventoryDataEditor : Editor
{
    SerializedProperty inventoryModeProperty;
    SerializedProperty maxSlotsProperty;
    SerializedProperty itemsProperty;

    private void OnEnable()
    {
        inventoryModeProperty = serializedObject.FindProperty("_inventoryMode");
        maxSlotsProperty = serializedObject.FindProperty("_maxSlots");
        itemsProperty = serializedObject.FindProperty("_items");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(inventoryModeProperty);

        if (inventoryModeProperty.enumValueIndex == (int)InventoryData.InventoryMode.InventoryPlayer)
        {
            EditorGUILayout.PropertyField(maxSlotsProperty, new GUIContent("Maximum number of slots"));
        }

        EditorGUILayout.PropertyField(itemsProperty, new GUIContent("Items"), true);

        serializedObject.ApplyModifiedProperties();
    }
}