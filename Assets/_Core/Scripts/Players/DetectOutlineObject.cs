using MoonlitMixes.CookingMachine;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Inventory;
using MoonlitMixes.Player;
using MoonlitMixes.Scene;
using UnityEngine;

public class DetectOutlineObject : MonoBehaviour
{
    [SerializeField] private PlayerHoldItem _playerHoldItem;
    [SerializeField] private PlayerInteraction _playerInteraction;

    private void Awake()
    {
        _playerInteraction = FindFirstObjectByType<PlayerInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutlineObj"))
        {
            if (_playerInteraction.CanInteract && other.TryGetComponent<CauldronMixing>(out CauldronMixing cauldronMixing))
            {
                int i = (int)char.GetNumericValue(other.name, other.name.Length - 1);
                if (i == 1) _playerHoldItem.ActivateOutline(OutlineName.Cauldron1);
                else if (i == 2) _playerHoldItem.ActivateOutline(OutlineName.Cauldron2);
                else if (i == 3) _playerHoldItem.ActivateOutline(OutlineName.Cauldron3);
            }
            else if (_playerInteraction.CanInteract && other.TryGetComponent<KitchenMortar>(out KitchenMortar kitchenMortar))
            {
                _playerHoldItem.ActivateOutline(OutlineName.Mortar);
            }
            else if (_playerInteraction.CanInteract && other.TryGetComponent<CuttingBoard>(out CuttingBoard cuttingBoard))
            {
                _playerHoldItem.ActivateOutline(OutlineName.CuttingTable);
            }
            else if (_playerInteraction.CanInteract && other.TryGetComponent<WaitingTable>(out WaitingTable waitingTable))
            {
                _playerHoldItem.ActivateOutline(OutlineName.WaitingTable);
            }
            else if (other.TryGetComponent<InventoryStoragePotion>(out InventoryStoragePotion inventoryStoragePotion))
            {
                _playerHoldItem.ActivateOutline(OutlineName.Cellar);
            }
            else if (other.TryGetComponent<DoorSceneChange>(out DoorSceneChange doorSceneChange))
            {
                _playerHoldItem.ActivateOutline(OutlineName.Ladder);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OutlineObj"))
        {
            if (other.TryGetComponent<CauldronMixing>(out CauldronMixing cauldronMixing) /*&& OutlineActivatorInLabo.hasActivatedInLabo*/)
            {
                int i = (int)char.GetNumericValue(other.name, other.name.Length - 1);
                _playerHoldItem.DeactivateOutline(_playerHoldItem.CauldronsOutline[i - 1]);
            }
            else if (other.TryGetComponent<KitchenMortar>(out KitchenMortar kitchenMortar))
            {
                _playerHoldItem.DeactivateOutline(_playerHoldItem.MortarOutline);
            }
            else if (other.TryGetComponent<CuttingBoard>(out CuttingBoard cuttingBoard))
            {
                _playerHoldItem.DeactivateOutline(_playerHoldItem.CuttingBoardOutline);
            }
            else if (other.TryGetComponent<WaitingTable>(out WaitingTable waitingTable))
            {
                _playerHoldItem.DeactivateOutline(_playerHoldItem.TableOutline);
            }
            else if (other.TryGetComponent<InventoryStoragePotion>(out InventoryStoragePotion inventoryStoragePotion))
            {
                _playerHoldItem.DeactivateOutline(_playerHoldItem.CellarOutline);
            }
            else if (other.TryGetComponent<DoorSceneChange>(out DoorSceneChange doorSceneChange))
            {
                _playerHoldItem.DeactivateOutline(_playerHoldItem.LadderOutline);
            }
        }
    }
}