using MoonlitMixes.Datas;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Interactions
{
    using ExplorationTools;
    
    public class InteractionObject : MonoBehaviour, IInteraction
    {
        [SerializeField] private ToolType _toolType;
        [SerializeField] private Sprite[] _spriteArray;
        [SerializeField] private InputCommand _inputCommand;
        [SerializeField] private Image _interactionImage;
        
        private void UpdateInputUI()
        {
            if (_interactionImage == null)
                return;
            
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

        protected virtual void Awake()
        {
            UpdateInputUI();
        }

        public ToolType GetToolType()
        {
            return _toolType;
        }

        public virtual void EnableUI()
        {
            _interactionImage?.gameObject.SetActive(true);
        }

        public virtual void DisableUI()
        {
            _interactionImage?.gameObject.SetActive(false);
        }
        
        public virtual GameObject Interact() { return gameObject; }
    }
}