using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class RouletteSelectionTools : MonoBehaviour
{
    [SerializeField] private ToolListData toolListData;
    [SerializeField] private Image[] toolSlots;
    [SerializeField] private List<GameObject> toolGameObjects; 

    private int currentToolIndex = 0;
    private List<ToolData> tools; 

    private void Start()
    {
        tools = toolListData != null ? toolListData.ToolList : new List<ToolData>();

        if (tools.Count == 0 || toolGameObjects.Count == 0 || tools.Count != toolGameObjects.Count)
        {
            Debug.LogError("Mismatch between ToolData and GameObjects! Ensure both lists have the same number of items.");
            return;
        }

        UpdateToolSlots();
        UpdateActiveTool();
    }

    public void ChangeTool(InputAction.CallbackContext ctx)
    {
        if (tools.Count == 0 || toolGameObjects.Count == 0) return;

        float input = ctx.ReadValue<float>();

        if (input > 0)
        {
            currentToolIndex = (currentToolIndex + 1) % tools.Count;
        }
        else if (input < 0)
        {
            currentToolIndex = (currentToolIndex - 1 + tools.Count) % tools.Count;
        }

        UpdateToolSlots();
        UpdateActiveTool();
    }

    private void UpdateToolSlots()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            if (i < tools.Count)
            {
                int toolIndex = (currentToolIndex + i) % tools.Count;

                toolSlots[i].sprite = tools[toolIndex].ItemSprite;
                toolSlots[i].gameObject.SetActive(true);
            }
            else
            {
                toolSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateActiveTool()
    {
        for (int i = 0; i < toolGameObjects.Count; i++)
        {
            toolGameObjects[i].SetActive(i == currentToolIndex);
        }
    }
}