using MoonlitMixes.Datas;
using MoonlitMixes.Dialogue.Effect;
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
        [SerializeField] private SpeakerEffect[] _textSpeakerEffects;
        [SerializeField] private SpeakerEffect[] _spriteSpeakerEffects;

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
            _inputActionAsset = _playerInput?.actions;

            if (_playerInput == null || _inputActionAsset == null)
            {
                Debug.LogError("PlayerInput or InputActionAsset is missing in DialogueController!");
            }

            for (int i = 0; i < _spriteSpeakerEffects.Length; i++)
            {
                if (i < _textBoxes.Length && _spriteSpeakerEffects[i] != null)
                {
                    _spriteSpeakerEffects[i].SetLinkedText(_textBoxes[i]);
                }
            }
        }

        public void StartDialogue(DialogueData dialogue)
        {
            if (_inputActionAsset == null) return;

            _originalActionMap = _inputActionAsset.FindActionMap("Player");
            var dialogueActionMap = _inputActionAsset.FindActionMap("Dialogue");

            if (_originalActionMap == null || dialogueActionMap == null)
            {
                Debug.LogError("Missing ActionMap: 'Player' or 'Dialogue'");
                return;
            }

            _panelDialogue.SetActive(true);
            dialogueActionMap.Enable();

            _currentDialogue = dialogue;
            if (_currentDialogue?.Lines == null || _currentDialogue.Lines.Length == 0)
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
            if (_dialogueIndex >= _currentDialogue.Lines.Length)
            {
                EndDialogue();
                return;
            }

            DialogueLineData line = _currentDialogue.Lines[_dialogueIndex];
            int speakerIndex = line.SpeakerIndex;

            if (speakerIndex < 0 || speakerIndex >= _textBoxes.Length)
            {
                Debug.LogWarning($"SpeakerIndex {speakerIndex} is out of bounds!");
                _dialogueIndex++;
                if (_dialogueIndex >= _currentDialogue.Lines.Length)
                {
                    EndDialogue();
                }
                else
                {
                    StartCoroutine(DisplayNextDialogueWithDelay());
                }
                return;
            }

            for (int i = 0; i < _spriteSpeakerEffects.Length; i++)
            {
                var spriteEffect = _spriteSpeakerEffects[i];
                var textEffect = _textSpeakerEffects[i];

                if (spriteEffect != null && textEffect != null)
                {
                    spriteEffect.SetDialogueLineData(line);
                    textEffect.SetDialogueLineData(line);

                    if (i == speakerIndex)
                    {
                        spriteEffect.ResetEffect();
                        textEffect.ResetEffect();

                        // On applique FadeIn *avant* le texte
                        StartCoroutine(PlayEffectsBeforeText(line, spriteEffect, textEffect, _textBoxes[speakerIndex]));
                    }
                    if (!line.IsFadeEffect)
                    {
                        spriteEffect.DimEffect();
                        textEffect.DimEffect();
                    }
                }
            }

            _dialogueIndex++;
        }

        private IEnumerator PlayEffectsBeforeText(DialogueLineData line, SpeakerEffect spriteEffect, SpeakerEffect textEffect, TMP_Text textBox)
        {
            if (line.Effect == SpeakerEffectType.FadeIn)
            {
                yield return spriteEffect.PlayEffect(line.Effect);
                yield return textEffect.PlayEffect(line.Effect);
            }

            WriteText(line.Text, textBox);
            yield return StartCoroutine(TypeText(line.Text, textBox));

            if (line.Effect == SpeakerEffectType.FadeOut)
            {
                yield return spriteEffect.PlayEffect(line.Effect);
                yield return textEffect.PlayEffect(line.Effect);
            }
        }

        private IEnumerator DisplayNextDialogueWithDelay()
        {
            yield return null;
            DisplayNextDialogue();
        }

        private void WriteText(string text, TMP_Text textBox)
        {
            textBox.maxVisibleCharacters = 0;
            textBox.text = text;
        }

        private IEnumerator TypeText(string text, TMP_Text textBox)
        {
            _isTyping = true;
            for (int i = 0; i < text.Length; ++i)
            {
                if (_isSkipText)
                {
                    textBox.maxVisibleCharacters = text.Length;
                    _isSkipText = false;
                    break;
                }

                textBox.maxVisibleCharacters++;
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