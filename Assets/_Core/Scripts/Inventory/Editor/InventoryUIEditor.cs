using MoonlitMixes.Datas;
using MoonlitMixes.Inventory;
using UnityEditor;

namespace MoonlitMixes.Editor
{
    [CustomEditor(typeof(InventoryUI))]
    public class InventoryUIEditor : UnityEditor.Editor
    {
        SerializedProperty inventoryProperty;
        SerializedProperty slotPrefabProperty;
        SerializedProperty inventoryReceivesProperty;
        SerializedProperty scaleItemProperty;
        SerializedProperty emptyItemProperty;
        SerializedProperty inventoryCellarProperty;

        private void OnEnable()
        {
            inventoryProperty = serializedObject.FindProperty("_inventory");
            slotPrefabProperty = serializedObject.FindProperty("_slotPrefab");
            inventoryReceivesProperty = serializedObject.FindProperty("_inventoryReceives");
            scaleItemProperty = serializedObject.FindProperty("_scaleItem");
            emptyItemProperty = serializedObject.FindProperty("_emptyItem");
            inventoryCellarProperty = serializedObject.FindProperty("_inventoryCellar");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(inventoryProperty);
            EditorGUILayout.PropertyField(scaleItemProperty);
            EditorGUILayout.PropertyField(emptyItemProperty);
            EditorGUILayout.PropertyField(inventoryCellarProperty);

            InventoryData inventory = inventoryProperty.objectReferenceValue as InventoryData;

            if (inventory != null)
            {
                if (inventory.Mode == InventoryMode.InventoryCellar)
                {
                    EditorGUILayout.PropertyField(slotPrefabProperty);
                }

                if (inventory.Mode != InventoryMode.InventoryCellar)
                {
                    EditorGUILayout.PropertyField(inventoryReceivesProperty);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}