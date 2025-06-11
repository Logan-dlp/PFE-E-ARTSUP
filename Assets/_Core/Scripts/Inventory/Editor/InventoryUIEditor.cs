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

        private void OnEnable()
        {
            inventoryProperty = serializedObject.FindProperty("_inventory");
            slotPrefabProperty = serializedObject.FindProperty("_slotPrefab");
            inventoryReceivesProperty = serializedObject.FindProperty("_inventoryReceives");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(inventoryProperty);

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