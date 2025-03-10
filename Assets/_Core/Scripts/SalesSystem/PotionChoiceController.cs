using UnityEngine;
using System;
using MoonlitMixes.Potion;

public class PotionChoiceController : MonoBehaviour
{
    [SerializeField] private GameObject _potionChoicePanel;
    [SerializeField] private PotionInventory _potionInventory;

    public static event Action<string> OnPotionChoiceSelected;

    private void Start()
    {
        _potionChoicePanel.SetActive(false);
        DialogueController.OnDialogueFinished += ShowPotionChoices;
    }

    public void ShowPotionChoices()
    {
        _potionChoicePanel.SetActive(true);

        _potionInventory.UpdatePotionCanvas();
    }

    private void SelectPotion(string potionName)
    {
        _potionChoicePanel.SetActive(false);
        OnPotionChoiceSelected?.Invoke(potionName);
    }
}