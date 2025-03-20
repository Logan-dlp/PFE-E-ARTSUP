using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UI;

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
    
        private bool _isTextIsWritten = false;
        private bool _isSkipText = false;
        
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
            _panelDialogue.SetActive(false);
        }
    
        public void LaunchDialogue(DialogueScriptableObject dialogue)
        {
            _panelDialogue.SetActive(true);
            ClearDialogue();
            NextDialogue(dialogue);
        }
    
        public void NextDialogue(DialogueScriptableObject dialogue)
        {
            int _dialogueIterator = 0;
            
            if (_dialogueIterator >= dialogue.dialogueSection.Length)
            {
                EndDialogue();
                return;
            }
            
            if (_isTextIsWritten)
            {
                _isSkipText = true;
                return;
            }
            
            if (dialogue.dialogueSection[_dialogueIterator].isPlayer)
            {
                _imagePlayer.sprite = dialogue.dialogueSection[_dialogueIterator].sprite;
                _imagePlayer.preserveAspect = true;
                WriteText(dialogue.dialogueSection[_dialogueIterator].dialogue, _textPlayer);
            }
            else
            {
                _imageNPC.sprite = dialogue.dialogueSection[_dialogueIterator].sprite;
                _imageNPC.preserveAspect = true;
                WriteText(dialogue.dialogueSection[_dialogueIterator].dialogue, _textNPC);
            }
            
            _dialogueIterator++;
        }
    
        public void EndDialogue()
        {
            ClearDialogue();
            _panelDialogue.SetActive(false);
            OnDialogueFinished?.Invoke();
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
            _isTextIsWritten = true;
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
            _isTextIsWritten = false;
        }
    }
}