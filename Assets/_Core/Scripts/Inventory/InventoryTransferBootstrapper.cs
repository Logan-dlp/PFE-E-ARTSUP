using UnityEngine;
using MoonlitMixes.Datas;
using MoonlitMixes.Item;

namespace MoonlitMixes.Boot
{
    public class InventoryTransferBootstrapper : MonoBehaviour
    {
        [SerializeField] private InventoryData _playerInventory;
        [SerializeField] private InventoryData _chestInventory;

        private InventoryTransferOnDeathHandler _transferHandler;

        private void Awake()
        {
            _transferHandler = new InventoryTransferOnDeathHandler(_playerInventory, _chestInventory);
        }

        private void OnDestroy()
        {
            _transferHandler.Dispose();
        }
    }
}