using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryData))]
public class InventoryDataEditor : Editor
{
    private void OnEnable()
    {
        EditorApplication.update += UpdateInspector;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateInspector;
    }

    private void UpdateInspector()
    {
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        InventoryData inventoryData = (InventoryData)target;

        inventoryData.Mode = (InventoryData.InventoryMode)EditorGUILayout.EnumPopup("Inventory mode", inventoryData.Mode);

        if (inventoryData.Mode == InventoryData.InventoryMode.InventoryPlayer)
        {
            inventoryData.MaxSlots = EditorGUILayout.IntField("Maximum number of slots", inventoryData.MaxSlots);
        }

        SerializedProperty itemsProperty = serializedObject.FindProperty("_items");
        EditorGUILayout.PropertyField(itemsProperty, new GUIContent("Items"), true);

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
