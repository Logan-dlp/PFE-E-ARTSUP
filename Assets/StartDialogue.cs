using System;
using MoonlitMixes.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartDialogue : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;
    
    void Start()
    {
        FindFirstObjectByType<PlayerInput>().SwitchCurrentActionMap("Dialogue");
        _dialogueController.LaunchDialogue(_dialogueScriptableObject);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Debug.Log(FindFirstObjectByType<PlayerInput>().currentActionMap);
    }
}
