using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.ExplorationTools;
using MoonlitMixes.Datas;

public class RouletteSelectionTools : MonoBehaviour
{
    public static event System.Action OnToolChanged;

    [SerializeField] private Image[] _toolSlots;
    public Image[] ToolSlots
    {
        get => _toolSlots;
    }
    [SerializeField] private List<GameObject> _toolGameObjects;
    public List<GameObject> ToolGameObjects
    {
        get => _toolGameObjects;
    }

    [SerializeField] private ToolAcquired _toolAcquired;
    [SerializeField] private GameObject _toolPivot;

    private int _currentToolIndex = 0;
    private List<ToolData> _tools;

    public ToolType CurrentToolType { get; private set; }

    private void Start()
    {
        _tools = new List<ToolData>();

        _toolGameObjects = new List<GameObject>();

        GetAcquiredTools();
        UpdateToolSlots();
        UpdateActiveTool();
    }

    public void AddTool(ToolData newTool, GameObject toolPrefab)
    {
        GameObject tool = Instantiate(toolPrefab, _toolPivot.transform);

        _tools.Add(newTool);
        _toolGameObjects.Add(tool);
        
        toolPrefab.SetActive(false);

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

        OnToolChanged?.Invoke();
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
                _toolSlots[i].preserveAspect = true;
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
            if (i == _currentToolIndex)
            {
                _toolGameObjects[i].SetActive(true);
            }
            else
            {
                _toolGameObjects[i].SetActive(false);
            }
        }
    }

    private void GetAcquiredTools()
    {
        foreach (ToolAcquired.Tool tool in _toolAcquired.toolAcquiredArray)
        {
            AddTool(tool.toolData, tool.toolPrefab);
        }
    }
}