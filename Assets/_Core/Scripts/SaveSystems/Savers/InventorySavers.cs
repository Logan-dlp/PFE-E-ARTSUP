using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.SaveSystems.Savers
{
    public class InventorySavers : MonoBehaviour, ISerializable
    {
        [SerializeField] private InventoryData _inventoryData;
        
        public string Serialize()
        {
            return _inventoryData.Serialize();
        }

        public void Deserialize(string data)
        {
            _inventoryData.Deserialize(data);
        }
    }
}