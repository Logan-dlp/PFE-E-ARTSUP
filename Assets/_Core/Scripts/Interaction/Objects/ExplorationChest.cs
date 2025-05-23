using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Interactions.Objects
{
    using Datas;
    using Inventory;
    
    public class ExplorationChest : InteractionObject
    {
        [SerializeField] private Sprite[] _spriteArray;
        [SerializeField] private InputCommand _inputCommand;
        [SerializeField] private Image _interactionImage;
        
        private ChestInteraction _chestInteraction;

        private void Awake()
        {
            _chestInteraction = GetComponent<ChestInteraction>();
            UpdateInputUI();
        }

        private void UpdateInputUI()
        {
            switch (_inputCommand)
            {
                case InputCommand.A:
                    _interactionImage.sprite = _spriteArray[0];
                    break;
                case InputCommand.B:
                    _interactionImage.sprite = _spriteArray[1];
                    break;
                case InputCommand.X:
                    _interactionImage.sprite = _spriteArray[2];
                    break;
                case InputCommand.Y:
                    _interactionImage.sprite = _spriteArray[3];
                    break;
            }
        }

        public override void EnableUI()
        {
            _interactionImage.gameObject.SetActive(true);
        }

        public override void DisableUI()
        {
            _interactionImage.gameObject.SetActive(false);
        }

        public override void Interact()
        {
            _chestInteraction.OpenChest();
        }
    }
}