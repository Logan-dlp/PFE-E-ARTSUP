using UnityEngine;
using UnityEngine.UI;
using System;

public class PotionChoiceController : MonoBehaviour
{
    [SerializeField] private GameObject _potionChoicePanel;
    [SerializeField] private Transform _buttonsParent;

    public static event Action<string> OnPotionChoiceSelected;

    private void Start()
    {
        _potionChoicePanel.SetActive(false);

        Button[] buttons = _buttonsParent.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => SelectPotion(button.name));
        }

        DialogueController.OnDialogueFinished += ShowPotionChoices;
    }

    public void ShowPotionChoices()
    {
        _potionChoicePanel.SetActive(true);
    }

    private void SelectPotion(string potionName)
    {
        _potionChoicePanel.SetActive(false);
        OnPotionChoiceSelected?.Invoke(potionName);
    }
}
