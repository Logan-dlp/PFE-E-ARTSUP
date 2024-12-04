using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    SerializedProperty inventoryProperty;
    SerializedProperty slotPrefabProperty;

    private void OnEnable()
    {
        inventoryProperty = serializedObject.FindProperty("_inventory");
        slotPrefabProperty = serializedObject.FindProperty("_slotPrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(inventoryProperty);

        InventoryData inventory = inventoryProperty.objectReferenceValue as InventoryData;
        if (inventory != null && inventory.Mode != InventoryData.InventoryMode.InventoryPlayer)
        {
            EditorGUILayout.PropertyField(slotPrefabProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}