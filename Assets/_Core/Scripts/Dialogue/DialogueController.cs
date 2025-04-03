using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

namespace MoonlitMixes.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private static DialogueController _instance;
        public static DialogueController Instance => _instance;

        public static event System.Action OnDialogueFinished;

        [SerializeField] private GameObject _panelDialogue;
        [SerializeField] private TMP_Text[] _textBoxes;  // Les 2 blocs de texte
        [SerializeField] private Image[] _imageSpeaker; // Les 2 images associées aux blocs de texte
        [SerializeField] private SpeakerEffect[] _allSpeakers; // Les 4 personnages max dans la scène

        private Dictionary<int, SpeakerEffect> _activeSpeakers = new Dictionary<int, SpeakerEffect>();
        private DialogueData _currentDialogue;
        private int _dialogueIndex = 0;
        private bool _isTyping = false;
        private Dictionary<int, int> _speakerToTextBox = new Dictionary<int, int>(); // Associe les personnages aux blocs de texte


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void StartDialogue(DialogueData dialogue)
        {
            _currentDialogue = dialogue;
            if (_currentDialogue == null) return;

            _panelDialogue.SetActive(true);

            Debug.Log("Le panneau de dialogue est activé : " + _panelDialogue.activeSelf);

            _dialogueIndex = 0;

            MapSpeakersToTextBoxes();
            DisplayNextDialogue();
        }

        private void MapSpeakersToTextBoxes()
        {
            _activeSpeakers.Clear();
            _speakerToTextBox.Clear();

            List<int> assignedSpeakers = new List<int>();

            for (int i = 0; i < _allSpeakers.Length; i++)
            {
                SpeakerEffect speaker = _allSpeakers[i];
                if (speaker.gameObject.activeSelf) // On prend que les personnages actifs
                {
                    _activeSpeakers.Add(i, speaker);
                    assignedSpeakers.Add(i);
                }
            }

            if (assignedSpeakers.Count > 2)
            {
                Debug.LogWarning("Plus de 2 personnages parlent en même temps, ça peut être confus.");
            }

            for (int i = 0; i < Mathf.Min(assignedSpeakers.Count, 2); i++)
            {
                _speakerToTextBox.Add(assignedSpeakers[i], i);
            }
        }

        public void DisplayNextDialogue()
        {
            Debug.Log("Affichage de la prochaine ligne de dialogue...");

            if (_dialogueIndex >= _currentDialogue.lines.Length)
            {
                EndDialogue();
                return;
            }

            DialogueLineData line = _currentDialogue.lines[_dialogueIndex];

            int speakerIndex = line.speakerIndex;
            int textBoxIndex = _speakerToTextBox.ContainsKey(speakerIndex) ? _speakerToTextBox[speakerIndex] : 0;

            UpdateSpeakerEffects(speakerIndex, line.effect);

            _textBoxes[textBoxIndex].text = "";
            _imageSpeaker[textBoxIndex].sprite = line.speakerSprite;
            StartCoroutine(TypeText(line.text, _textBoxes[textBoxIndex]));

            _dialogueIndex++;
        }

        private void UpdateSpeakerEffects(int activeIndex, SpeakerEffectType effect)
        {
            foreach (var speaker in _activeSpeakers)
            {
                if (speaker.Key == activeIndex)
                {
                    speaker.Value.ApplyEffect(effect);
                }
                else
                {
                    speaker.Value.DimEffect();
                }
            }
        }

        private IEnumerator TypeText(string text, TMP_Text textBox)
        {
            _isTyping = true;
            textBox.text = "";

            foreach (char letter in text)
            {
                textBox.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            _isTyping = false;
        }

        public void EndDialogue()
        {
            _panelDialogue.SetActive(false);
            OnDialogueFinished?.Invoke();
        }

        public void OnNextDialoguePressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DisplayNextDialogue();
            }
        }
    }
}