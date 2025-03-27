using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSelectionTools : MonoBehaviour
{
    [SerializeField] private ToolListData _toolListData;
    [SerializeField] private Image[] _toolSlots;
    [SerializeField] private List<GameObject> _toolGameObjects;

    private int _currentToolIndex = 0;
    private List<ToolData> _tools;

    public ToolType CurrentToolType { get; private set; }

    private void Start()
    {
        _tools = _toolListData != null ? _toolListData.ToolList : new List<ToolData>();

        if (_tools.Count == 0 || _toolGameObjects.Count == 0 || _tools.Count != _toolGameObjects.Count)
        {
            Debug.LogError("Mismatch entre ToolData et GameObjects !");
            return;
        }

        CurrentToolType = _tools[_currentToolIndex].ToolType;
        UpdateToolSlots();
        UpdateActiveTool();
    }

    public void ChangeTool(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_tools.Count == 0 || _toolGameObjects.Count == 0) return;

        float input = ctx.ReadValue<float>();

        if (input > 0)
        {
            _currentToolIndex = (_currentToolIndex + 1) % _tools.Count;
        }
        else if (input < 0)
        {
            _currentToolIndex = (_currentToolIndex - 1 + _tools.Count) % _tools.Count;
        }

        CurrentToolType = _tools[_currentToolIndex].ToolType;

        UpdateToolSlots();
        UpdateActiveTool();
    }

    private void UpdateToolSlots()
    {
        for (int i = 0; i < _toolSlots.Length; i++)
        {
            if (i < _tools.Count)
            {
                int toolIndex = (_currentToolIndex + i) % _tools.Count;
                _toolSlots[i].sprite = _tools[toolIndex].ItemSprite;
                _toolSlots[i].gameObject.SetActive(true);
            }
            else
            {
                _toolSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateActiveTool()
    {
        for (int i = 0; i < _toolGameObjects.Count; i++)
        {
            _toolGameObjects[i].SetActive(i == _currentToolIndex);
        }
    }
}