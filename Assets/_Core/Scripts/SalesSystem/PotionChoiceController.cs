using UnityEngine;
using System;
using MoonlitMixes.Potion;

public class PotionChoiceController : MonoBehaviour
{
    [SerializeField] private GameObject _potionChoicePanel;
    [SerializeField] private PotionInventory _potionInventory;

    private string _selectedPotionName;
    public string SelectedPotionName => _selectedPotionName;

    public static event Action<string> OnPotionChoiceSelected;

    private void Start()
    {
        _potionChoicePanel.SetActive(false);
    }

    public void ShowPotionChoices()
    {
        _potionChoicePanel.SetActive(true);

        if (_potionInventory.PotionList.Count > 0)
        {
            _selectedPotionName = _potionInventory.PotionList[0].Recipe.RecipeName;
            Debug.Log($"Potion choisie par le PNJ : {_selectedPotionName}");
        }

        _potionInventory.UpdatePotionCanvas();
    }

    public void SelectPotion(string potionName)
    {
        if (_selectedPotionName == potionName)
        {
            Debug.Log("Bonne potion choisie !");
            OnPotionChoiceSelected?.Invoke(potionName);
        }
        else
        {
            Debug.Log("Mauvaise potion, essayez encore !");
            OnPotionChoiceSelected?.Invoke(potionName);
        }

        _potionChoicePanel.SetActive(false);
    }
}