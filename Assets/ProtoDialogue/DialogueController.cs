using TMPro;
using UnityEngine;
using System;
using System.Collections;
using MoonlitMixes.Inputs;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private static DialogueController _instance;
        public static DialogueController Instance => _instance;
        
        public static event Action OnDialogueFinished;
        
        [SerializeField] private GameObject _panelDialogue;
        [SerializeField] private TMP_Text _textPlayer;
        [SerializeField] private TMP_Text _textNPC;
        [SerializeField] private Image _imagePlayer;
        [SerializeField] private Image _imageNPC;
        [SerializeField] private float letterDelay = .05f;
        
        private bool _isSkipText = false;
        private DialogueScriptableObject _dialogueScriptableObject;
        private int _dialogueIterator;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            //_panelDialogue.SetActive(false);
        }
    
        public void LaunchDialogue(DialogueScriptableObject dialogue)
        {
            _dialogueIterator = 0;
            _dialogueScriptableObject = dialogue;
            _panelDialogue.SetActive(true);
            ClearDialogue();
            NextDialogue();
        }

        public void StartNextDialogueFromController(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                NextDialogue();
            }
        }
    
        public void NextDialogue()
        {
            if (_dialogueIterator >= _dialogueScriptableObject.dialogueSection.Length)
            {
                EndDialogue();
                return;
            }
            
            if(_dialogueScriptableObject.dialogueSection[_dialogueIterator].dialogue.Length == 0)
            {
                if(_dialogueScriptableObject.dialogueSection[_dialogueIterator].isPlayer)
                {
                    _imagePlayer.sprite = _dialogueScriptableObject.dialogueSection[_dialogueIterator].sprite;
                }
                else
                {
                    _imageNPC.sprite = _dialogueScriptableObject.dialogueSection[_dialogueIterator].sprite;
                }
                _dialogueIterator++;
            }

            /*if (_isTextIsWritten)
            {
                Debug.Log("");
                _isSkipText = true;
                return;
            }*/
            
            if (_dialogueScriptableObject.dialogueSection[_dialogueIterator].isPlayer)
            {
                _imagePlayer.sprite = _dialogueScriptableObject.dialogueSection[_dialogueIterator].sprite;
                //_imagePlayer.preserveAspect = true;
                WriteText(_dialogueScriptableObject.dialogueSection[_dialogueIterator].dialogue, _textPlayer);
            }
            else
            {
                _imageNPC.sprite = _dialogueScriptableObject.dialogueSection[_dialogueIterator].sprite;
                //_imageNPC.preserveAspect = true;
                WriteText(_dialogueScriptableObject.dialogueSection[_dialogueIterator].dialogue, _textNPC);
            }

            _dialogueIterator++;
        }
    
        public void EndDialogue()
        {
            ClearDialogue();
            _panelDialogue.SetActive(false);
            OnDialogueFinished?.Invoke();
            InputManager.Instance.SwitchActionMap("Player");
        }
    
        private void ClearDialogue()
        {
            _textPlayer.text = "";
            _textNPC.text = "";
        }
    
    
        private void WriteText(string text, TMP_Text textBox)
        {
            textBox.maxVisibleCharacters = 0;
            textBox.text = text;
            StartCoroutine(TypeText(textBox));
        }
    
        private IEnumerator TypeText(TMP_Text textBox)
        {
            for (int i = 0; i < textBox.text.Length; ++i)
            {
                textBox.maxVisibleCharacters++;
                yield return new WaitForSeconds(letterDelay);
                if (_isSkipText == true)
                {
                    textBox.maxVisibleCharacters = textBox.text.Length;
                    _isSkipText = false;
                    break;
                }
            }
        }
    }
}