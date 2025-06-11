using MoonlitMixes.Datas;
using MoonlitMixes.Events;

namespace MoonlitMixes.Item
{
    public class InventoryTransferOnDeathHandler
    {
        private readonly InventoryData _playerInventory;
        private readonly InventoryData _chestInventory;

        public InventoryTransferOnDeathHandler(InventoryData playerInventory, InventoryData coffinInventory)
        {
            _playerInventory = playerInventory;
            _chestInventory = coffinInventory;

            PlayerDeathEventDispatcher.OnPlayerDeath += HandleTransfer;
        }

        private void HandleTransfer()
        {
            if (_playerInventory.Items.Count == 0) return;

            for (int i = _playerInventory.Items.Count - 1; i >= 0; i--)
            {
                if (_chestInventory.Items.Count >= _chestInventory.MaxSlots) break;

                _chestInventory.Items.Add(_playerInventory.Items[i]);
                _playerInventory.Items.RemoveAt(i);
            }

            UnityEngine.Debug.Log("Inventaire transféré automatiquement dans le coffre de récupération.");
        }

        public void Dispose()
        {
            PlayerDeathEventDispatcher.OnPlayerDeath -= HandleTransfer;
        }
    }
}