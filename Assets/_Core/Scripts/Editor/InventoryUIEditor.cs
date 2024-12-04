using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventoryUI inventoryUI = (InventoryUI)target;

        SerializedObject serializedObject = new SerializedObject(target);

        SerializedProperty inventoryProperty = serializedObject.FindProperty("_inventory");
        SerializedProperty slotPrefabProperty = serializedObject.FindProperty("_slotPrefab");

        EditorGUILayout.PropertyField(inventoryProperty);

        InventoryData inventory = inventoryProperty.objectReferenceValue as InventoryData;

        if (inventory != null && inventory.Mode != InventoryData.InventoryMode.InventoryPlayer)
        {
            EditorGUILayout.PropertyField(slotPrefabProperty, new GUIContent("Slot Prefab"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}