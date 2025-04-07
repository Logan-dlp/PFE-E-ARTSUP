using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonlitMixes.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private static DialogueController _instance;
        public static DialogueController Instance => _instance;

        public static event System.Action OnDialogueFinished;

        [SerializeField] private GameObject _panelDialogue;
        [SerializeField] private float _letterDelay;
        [SerializeField] private TMP_Text[] _textBoxes;
        [SerializeField] private Image[] _imageSpeakers;
        [SerializeField] private SpeakerEffect[] _allSpeakers;

        private DialogueData _currentDialogue;
        private int _dialogueIndex = 0;
        private bool _isTyping = false;
        private bool _isSkipText = false;

        private PlayerInput _playerInput;
        private InputActionAsset _inputActionAsset;
        private InputActionMap _originalActionMap;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            _playerInput = FindFirstObjectByType<PlayerInput>();
            if (_playerInput == null)
            {
                Debug.LogError("PlayerInput not found in the scene!");
                return;
            }

            _inputActionAsset = _playerInput.actions;
            if (_inputActionAsset == null)
            {
                Debug.LogError("InputActionAsset is missing in PlayerInput!");
                return;
            }
        }

        public void StartDialogue(DialogueData dialogue)
        {
            if (_inputActionAsset == null)
            {
                Debug.LogError("InputActionAsset is not assigned!");
                return;
            }

            _originalActionMap = _inputActionAsset.FindActionMap("Player");
            if (_originalActionMap == null)
            {
                Debug.LogError("The 'Player' ActionMap was not found!");
                return;
            }

            var dialogueActionMap = _inputActionAsset.FindActionMap("Dialogue");
            if (dialogueActionMap == null)
            {
                Debug.LogError("The 'Dialogue' ActionMap was not found!");
                return;
            }

            _panelDialogue.SetActive(true);
            dialogueActionMap.Enable();

            _currentDialogue = dialogue;
            if (_currentDialogue == null || _currentDialogue.lines == null || _currentDialogue.lines.Length == 0)
            {
                Debug.LogError("Dialogue data is invalid or empty!");
                EndDialogue();
                return;
            }

            _dialogueIndex = 0;
            DisplayNextDialogue();
        }

        public void DisplayNextDialogue()
        {
            if (_dialogueIndex >= _currentDialogue.lines.Length)
            {
                EndDialogue();
                return;
            }

            DialogueLineData line = _currentDialogue.lines[_dialogueIndex];
            int speakerIndex = line._speakerIndex;

            if (speakerIndex < 0 || speakerIndex >= _textBoxes.Length)
            {
                Debug.LogWarning($"SpeakerIndex {speakerIndex} is out of bounds!");
                _dialogueIndex++;
                DisplayNextDialogue();
                return;
            }

            _imageSpeakers[speakerIndex].sprite = line._speakerSprite;

            WriteText(line._text, _textBoxes[speakerIndex]);

            StartCoroutine(TypeText(line._text, _textBoxes[speakerIndex]));

            _dialogueIndex++;
        }

        private void WriteText(string text, TMP_Text textBox)
        {
            textBox.maxVisibleCharacters = 0;
            textBox.text = text;
        }

        private IEnumerator TypeText(string text, TMP_Text textBox)
        {
            for (int i = 0; i < textBox.text.Length; ++i)
            {
                textBox.maxVisibleCharacters++;
                _isTyping = true;

                if (_isSkipText)
                {
                    textBox.maxVisibleCharacters = textBox.text.Length;
                    _isSkipText = false;
                    break;
                }

                yield return new WaitForSeconds(_letterDelay);
            }

            _isTyping = false;
        }

        public void EndDialogue()
        {
            _panelDialogue.SetActive(false);
            _inputActionAsset.FindActionMap("Dialogue")?.Disable();
            _originalActionMap?.Enable();

            foreach (var textBox in _textBoxes)
            {
                if (textBox != null)
                {
                    textBox.text = "";
                    textBox.maxVisibleCharacters = 0;
                }
            }

            OnDialogueFinished?.Invoke();
        }

        public void OnNextDialoguePressed(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                if (_isTyping)
                {
                    _isSkipText = true;
                }
                else
                {
                    DisplayNextDialogue();
                }
            }
        }
    }
}