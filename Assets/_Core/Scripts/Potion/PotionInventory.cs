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

                nameText.text = PotionList[i].Recipe.RecipeName;
                potionImage.sprite = PotionList[i].Recipe.PotionSprite;

                Button btn = newSlot.GetComponent<Button>();
                string potionName = PotionList[i].Recipe.RecipeName;
                btn.onClick.AddListener(() => OnPotionButtonClicked(potionName, confirmationPanel, confirmButton, cancelButton, btn));
            }
        }

        private void OnPotionButtonClicked(string potionName, GameObject confirmationPanel, Button yesButton, Button noButton, Button potionButton)
        {
            Debug.Log($"Potion sélectionnée: {potionName}, affichage du panneau de confirmation.");

            potionButton.interactable = false;

            confirmationPanel.SetActive(true);

            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => ConfirmPotionChoice(potionName, confirmationPanel));

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() => CancelPotionChoice(confirmationPanel, potionButton));
        }

        private void ConfirmPotionChoice(string potionName, GameObject confirmationPanel)
        {
            if (_potionChoiceController != null)
            {
                _potionChoiceController.SelectPotion(potionName);
            }

            RemovePotionFromList(potionName);

            UpdatePotionCanvas();

            confirmationPanel.SetActive(false);
        }


        private void CancelPotionChoice(GameObject confirmationPanel, Button potionButton)
        {
            potionButton.interactable = true;
            confirmationPanel.SetActive(false);
        }

        private void RemovePotionFromList(string potionName)
        {
            PotionResult potionToRemove = PotionList.Find(potion => potion.Recipe.RecipeName == potionName);
            if (potionToRemove != null)
            {
                PotionList.Remove(potionToRemove);
            }
        }
    }
}