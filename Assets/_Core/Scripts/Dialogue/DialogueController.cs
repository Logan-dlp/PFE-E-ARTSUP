using MoonlitMixes.Datas;
using MoonlitMixes.Dialogue.Effect;
using MoonlitMixes.Inputs;
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
        public static event System.Action OnDialogueClosed;
        public static event System.Action OnDialogueSkipped;

        [SerializeField] private GameObject _panelDialogue;
        [SerializeField] private float _letterDelay;
        [SerializeField] private TMP_Text[] _textBoxes;
        [SerializeField] private Image[] _imageSpeakers;
        [SerializeField] private SpeakerEffect[] _textSpeakerEffects;
        [SerializeField] private SpeakerEffect[] _spriteSpeakerEffects;

        [SerializeField] private GameObject _gameObjectA; // Nouveau GameObject A
        [SerializeField] private GameObject _gameObjectB; // Nouveau GameObject B

        private DialogueData _currentDialogue;
        private int _dialogueIndex = 0;
        private bool _isTyping = false;
        private bool _isSkipText = false;
        private bool _isEffectRunning = false;
        private bool _hasSkippedEffect = false;
        private bool _isLastDialogue = false;
        public bool IsLastDialogue { set { _isLastDialogue = value; } }

        private PlayerInput _playerInput;
        private InputActionAsset _inputActionAsset;
        private InputActionMap _originalActionMap;
        private EndGameDialogue _endGameDialogue;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;

            _playerInput = FindFirstObjectByType<PlayerInput>();
            _endGameDialogue = gameObject.GetComponent<EndGameDialogue>();
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

            _panelDialogue.SetActive(true);
            StartCoroutine(DelayInputChange());
            
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
            if (_isEffectRunning) return;

            if (_dialogueIndex >= _currentDialogue.Lines.Length)
            {
                EndDialogue();
                return;
            }

            DialogueLineData line = _currentDialogue.Lines[_dialogueIndex];
            int speakerIndex = line.SpeakerIndex;

            // --- Gestion activation/désactivation des GameObjects selon speakerIndex ---
            if (speakerIndex == 0 || speakerIndex == 2)
            {
                _gameObjectA.SetActive(true);
                _gameObjectB.SetActive(false);
            }
            else if (speakerIndex == 1 || speakerIndex == 3)
            {
                _gameObjectA.SetActive(false);
                _gameObjectB.SetActive(true);
            }
            else
            {
                _gameObjectA.SetActive(false);
                _gameObjectB.SetActive(false);
            }
            // ---------------------------------------------------------------------------

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

            // Affichage des sprites du speaker
            for (int i = 0; i < _imageSpeakers.Length; i++)
            {
                if (i == speakerIndex)
                {
                    if (line.SpeakerSprite != null)
                    {
                        if (_imageSpeakers[i].sprite != line.SpeakerSprite)
                        {
                            _imageSpeakers[i].sprite = line.SpeakerSprite;
                        }
                        _imageSpeakers[i].enabled = true;
                    }
                    else if (_imageSpeakers[i].sprite != null)
                    {
                        _imageSpeakers[i].enabled = true;
                    }
                }
            }

            // Dim des autres
            for (int i = 0; i < _spriteSpeakerEffects.Length; i++)
            {
                if (i == speakerIndex) continue;

                _spriteSpeakerEffects[i]?.DimEffect();
                _textSpeakerEffects[i]?.DimEffect();
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

                        StartCoroutine(PlayEffectsBeforeText(line, spriteEffect, textEffect, _textBoxes[speakerIndex]));
                    }
                }
            }

            _dialogueIndex++;
        }

        private IEnumerator PlayEffectsBeforeText(DialogueLineData line, SpeakerEffect spriteEffect, SpeakerEffect textEffect, TMP_Text textBox)
        {
            _isEffectRunning = true;

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

            _isEffectRunning = false;

            if (_hasSkippedEffect)
            {
                _hasSkippedEffect = false;
                DisplayNextDialogue();
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
            InputManager.Instance.SwitchActionMap("PlayerMovement");

            foreach (TMP_Text textBox in _textBoxes)
            {
                if (textBox != null)
                {
                    textBox.text = "";
                    textBox.maxVisibleCharacters = 0;
                }
            }

            foreach (var image in _imageSpeakers)
            {
                if (image != null)
                {
                    image.sprite = null;
                    image.enabled = false;
                }
            }

            // Désactivation des GameObjects à la fin du dialogue
            _gameObjectA.SetActive(false);
            _gameObjectB.SetActive(false);

            OnDialogueFinished?.Invoke();
            OnDialogueClosed?.Invoke();
            if (_isLastDialogue) _endGameDialogue.EndGame();
        }

        public void OnNextDialoguePressed(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            if (_isEffectRunning && !_hasSkippedEffect)
            {
                _hasSkippedEffect = true;

                OnDialogueSkipped?.Invoke();

                foreach (SpeakerEffect effect in _spriteSpeakerEffects)
                {
                    if (effect != null)
                        effect.SkipEffectNow = true;
                }

                foreach (SpeakerEffect effect in _textSpeakerEffects)
                {
                    if (effect != null)
                        effect.SkipEffectNow = true;
                }

                return;
            }

            if (_isTyping)
            {
                _isSkipText = true;
                return;
            }

            DisplayNextDialogue();
        }
        private IEnumerator DelayInputChange()
        {
            yield return new WaitForSeconds(.1f);
            InputManager.Instance.SwitchActionMap("Dialogue");
        }
    }


}