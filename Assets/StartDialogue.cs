using MoonlitMixes.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;
    
    void Start()
    {
        _dialogueController.LaunchDialogue(_dialogueScriptableObject);
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        FindFirstObjectByType<PlayerInput>().SwitchCurrentActionMap("Dialogue");
    }
}
