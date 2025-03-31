using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Dialogue
{
    public class StartDialogue : MonoBehaviour
    {
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;
    
        void Start()
        {
            InputManager.Instance.SwitchActionMap("Dialogue");
            _dialogueController.LaunchDialogue(_dialogueScriptableObject);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}