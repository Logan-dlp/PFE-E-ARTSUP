using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Potion
{
    public class PotionInventory : MonoBehaviour
    {
        [SerializeField] private PotionListData potionResultListData;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _slotContainer;

        private PotionChoiceController _potionChoiceController;
        private Dictionary<string, int> _potionPrices = new Dictionary<string, int>();
        private bool _isSelectionInProgress = false;
        private List<Button> _potionButtons = new List<Button>();

        public List<PotionResult> PotionList => potionResultListData.PotionResults;

        private void Start()
        {
            _potionChoiceController = FindObjectOfType<PotionChoiceController>();
        }

        public void UpdatePotionCanvas()
        {
            foreach (Transform child in _slotContainer)
            {
                Destroy(child.gameObject);
            }

            _potionButtons.Clear();

            for (int i = 0; i < PotionList.Count; i++)
            {
                GameObject newSlot = Instantiate(_slotPrefab, _slotContainer);
                TextMeshProUGUI nameText = newSlot.GetComponentInChildren<TextMeshProUGUI>();

                Image[] images = newSlot.GetComponentsInChildren<Image>();
                Image potionImage = null;

                foreach (Image img in images)
                {
                    if (img.gameObject != newSlot)
                    {
                        potionImage = img;
                        break;
                    }
                }

                GameObject confirmationPanel = newSlot.transform.Find("ConfirmationPanel").gameObject;
                Button confirmButton = confirmationPanel.transform.Find("ConfirmButton").GetComponent<Button>();
                Button cancelButton = confirmationPanel.transform.Find("CancelButton").GetComponent<Button>();

                PotionResult potion = PotionList[i];
                nameText.text = potion.Recipe.RecipeName;
                potionImage.sprite = potion.Recipe.PotionSprite;

                Button btn = newSlot.GetComponent<Button>();
                _potionButtons.Add(btn);
                btn.onClick.AddListener(() => OnPotionButtonClicked(potion, confirmationPanel, confirmButton, cancelButton, btn));
            }
        }

        private void OnPotionButtonClicked(PotionResult potion, GameObject confirmationPanel, Button yesButton, Button noButton, Button potionButton)
        {
            if (_isSelectionInProgress) return;

            Debug.Log($"Potion sélectionnée: {potion.Recipe.RecipeName}, affichage du panneau de confirmation.");

            _isSelectionInProgress = true;
            TogglePotionButtons(false);

            potionButton.interactable = false;
            confirmationPanel.SetActive(true);

            if (!_potionPrices.ContainsKey(potion.Recipe.RecipeName))
            {
                _potionPrices[potion.Recipe.RecipeName] = potion.Price;
            }

            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => ConfirmPotionChoice(potion, confirmationPanel));

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() => CancelPotionChoice(confirmationPanel, potionButton));
        }

        private void ConfirmPotionChoice(PotionResult potion, GameObject confirmationPanel)
        {
            if (_potionChoiceController != null)
            {
                _potionChoiceController.SelectPotion(potion.Recipe.RecipeName);
            }

            if (_potionPrices.TryGetValue(potion.Recipe.RecipeName, out int price))
            {
                Debug.Log($"Potion confirmée: {potion.Recipe.RecipeName}, Prix: {price}");
                _potionPrices.Remove(potion.Recipe.RecipeName);
            }

            RemovePotionFromList(potion.Recipe.RecipeName);
            UpdatePotionCanvas();
            confirmationPanel.SetActive(false);

            _isSelectionInProgress = false;
            TogglePotionButtons(true);
        }

        private void CancelPotionChoice(GameObject confirmationPanel, Button potionButton)
        {
            _isSelectionInProgress = false;
            potionButton.interactable = true;
            confirmationPanel.SetActive(false);
            TogglePotionButtons(true);
        }

        private void RemovePotionFromList(string potionName)
        {
            PotionResult potionToRemove = PotionList.Find(potion => potion.Recipe.RecipeName == potionName);
            if (potionToRemove != null)
            {
                PotionList.Remove(potionToRemove);
            }
        }

        private void TogglePotionButtons(bool state)
        {
            foreach (Button btn in _potionButtons)
            {
                btn.interactable = state;
            }
        }
    }
}