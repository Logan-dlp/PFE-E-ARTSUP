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
        SerializedProperty inventoryData;

        private void OnEnable()
        {
            inventoryProperty = serializedObject.FindProperty("_inventory");
            slotPrefabProperty = serializedObject.FindProperty("_slotPrefab");
            inventoryData = serializedObject.FindProperty("_inventoryReceives");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(inventoryProperty);

            InventoryData inventory = inventoryProperty.objectReferenceValue as InventoryData;

            if (inventory != null && inventory.Mode != InventoryData.InventoryMode.InventoryCellar)
            {
                EditorGUILayout.PropertyField(inventoryData);
            }

            if (inventory != null && inventory.Mode != InventoryData.InventoryMode.InventoryPlayer)
            {
                EditorGUILayout.PropertyField(slotPrefabProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}