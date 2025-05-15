using MoonlitMixes.Datas;
using MoonlitMixes.ExplorationTools;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public abstract class AInteractionUI : MonoBehaviour
    {
        [SerializeField] protected ToolType _toolType;
        [SerializeField] private Sprite[] _spriteArray;
        [SerializeField] private InputCommand _inputCommand;
        [SerializeField] private Image _interactionImage;

        private void Start()
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

        protected void ActivationInteractionUI(bool state)
        {
            _interactionImage.gameObject.SetActive(state);
        }
    }
}