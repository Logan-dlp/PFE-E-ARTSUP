using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class RouletteSelectionTools : MonoBehaviour
{
    [SerializeField] private ToolListData toolListData; 
    [SerializeField] private Image[] toolSlots;

    private void Start()
    {
        List<ToolData> tools = toolListData != null ? toolListData.ToolList : new List<ToolData>();

        int toolCount = tools.Count;

        for (int i = 0; i < toolSlots.Length; i++)
        {
            if (toolCount == 0)
            {
                toolSlots[i].gameObject.SetActive(false);
            }
            else if (toolCount == 1)
            {
                toolSlots[i].sprite = tools[0].ItemSprite;
                toolSlots[i].gameObject.SetActive(true);
            }
            else if (toolCount == 2)
            {
                toolSlots[i].sprite = tools[i % 2].ItemSprite;
                toolSlots[i].gameObject.SetActive(true);
            }
            else
            {
                toolSlots[i].sprite = tools[i].ItemSprite;
                toolSlots[i].gameObject.SetActive(true);
            }
        }
    }

    public void ChangeTool(InputAction.CallbackContext ctx)
    {
        if (toolListData != null) 
        { 

        }
    }
}