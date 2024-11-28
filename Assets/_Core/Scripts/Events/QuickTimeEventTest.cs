using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickTimeEventTest : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _qteSlot;
    [SerializeField] private List<Sprite> _buttonIcons;  // Liste flexible d'ic�nes
    [SerializeField] private Dictionary<string, Sprite> _iconMapping;  // Dictionnaire dynamique pour les boutons et ic�nes

    [SerializeField] private InputAction _inputAction;

    private void Awake()
    {
        // Initialize the InputsActions and bind to this script
        _inputAction = new InputAction(); 

        // Initialize the mapping of inputs to icons dynamically
        InitializeButtonIconMapping();
    }

    private void InitializeButtonIconMapping()
    {
        _iconMapping = new Dictionary<string, Sprite>();

        // Exemple d'ajout dynamique des actions (ici, tu peux adapter en fonction de tes besoins)
        if (_buttonIcons.Count > 0)
        {
            _iconMapping.Add("buttonEast", _buttonIcons.Count > 0 ? _buttonIcons[0] : null);
            _iconMapping.Add("buttonNorth", _buttonIcons.Count > 1 ? _buttonIcons[1] : null);
            _iconMapping.Add("buttonSouth", _buttonIcons.Count > 2 ? _buttonIcons[2] : null);
            _iconMapping.Add("buttonWest", _buttonIcons.Count > 3 ? _buttonIcons[3] : null);
            _iconMapping.Add("leftShoulder", _buttonIcons.Count > 4 ? _buttonIcons[4] : null);
            _iconMapping.Add("rightShoulder", _buttonIcons.Count > 5 ? _buttonIcons[5] : null);
            _iconMapping.Add("leftTrigger", _buttonIcons.Count > 6 ? _buttonIcons[6] : null);
            _iconMapping.Add("rightTrigger", _buttonIcons.Count > 7 ? _buttonIcons[7] : null);
        }
    }

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    // Callback pour l'action QTE
    public void OnQTEAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            string pressedKey = context.control.name;  // Le nom du bouton ou de l'action press�e

            // V�rifie si l'action press�e a une ic�ne associ�e
            if (_iconMapping.ContainsKey(pressedKey))
            {
                Sprite icon = _iconMapping[pressedKey];
                if (icon != null)
                {
                    _qteSlot.sprite = icon;  // Affiche l'ic�ne correspondante
                    _qteSlot.enabled = true;
                }
                else
                {
                    Debug.LogWarning($"No icon mapped for input: {pressedKey}");
                    _qteSlot.enabled = false;  // Masque le slot si pas d'ic�ne
                }
            }
        }
    }
}