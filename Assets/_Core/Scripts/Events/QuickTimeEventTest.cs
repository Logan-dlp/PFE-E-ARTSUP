using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickTimeEventTest : MonoBehaviour, InputsActions.IQTEActions
{
    [Header("UI Elements")]
    [SerializeField] private Image _qteSlot;
    [SerializeField] private List<Sprite> _buttonIcons;  // Liste flexible d'icônes
    [SerializeField] private Dictionary<string, Sprite> _iconMapping;  // Dictionnaire dynamique pour les boutons et icônes

    [SerializeField] private InputsActions _inputsActions;

    private void Awake()
    {
        // Initialize the InputsActions and bind to this script
        _inputsActions = new InputsActions();
        _inputsActions.QTE.SetCallbacks(this);

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
        _inputsActions.Enable();
    }

    private void OnDisable()
    {
        _inputsActions.Disable();
    }

    // Callback pour l'action QTE
    public void OnQTEAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            string pressedKey = context.control.name;  // Le nom du bouton ou de l'action pressée

            // Vérifie si l'action pressée a une icône associée
            if (_iconMapping.ContainsKey(pressedKey))
            {
                Sprite icon = _iconMapping[pressedKey];
                if (icon != null)
                {
                    _qteSlot.sprite = icon;  // Affiche l'icône correspondante
                    _qteSlot.enabled = true;
                }
                else
                {
                    Debug.LogWarning($"No icon mapped for input: {pressedKey}");
                    _qteSlot.enabled = false;  // Masque le slot si pas d'icône
                }
            }
        }
    }
}