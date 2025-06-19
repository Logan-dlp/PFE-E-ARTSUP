using MoonlitMixes.Datas;
using MoonlitMixes.Inventory;
using UnityEditor;

namespace MoonlitMixes.Editor
{
    [CustomEditor(typeof(InventoryUI))]
    public class InventoryUIEditor : UnityEditor.Editor
    {
        SerializedProperty inventoryProperty;
        SerializedProperty inventoryExtensionList;
        SerializedProperty slotPrefabProperty;
        SerializedProperty inventoryReceivesProperty;
        SerializedProperty scaleItemProperty;
        SerializedProperty emptyItemProperty;

        private void OnEnable()
        {
            inventoryProperty = serializedObject.FindProperty("_inventory");
            inventoryExtensionList = serializedObject.FindProperty("_inventoryExtensionList");
            slotPrefabProperty = serializedObject.FindProperty("_slotPrefab");
            inventoryReceivesProperty = serializedObject.FindProperty("_inventoryReceives");
            scaleItemProperty = serializedObject.FindProperty("_scaleItem");
            emptyItemProperty = serializedObject.FindProperty("_emptyItem");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(inventoryProperty);
            EditorGUILayout.PropertyField(inventoryExtensionList);
            EditorGUILayout.PropertyField(scaleItemProperty);
            EditorGUILayout.PropertyField(emptyItemProperty);

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