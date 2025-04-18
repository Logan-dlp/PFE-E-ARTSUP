using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MoonlitMixes.Datas;
using UnityEngine.InputSystem;

namespace MoonlitMixes.ExplorationTools
{
    public class RouletteSelectionTools : MonoBehaviour
    {
        public ToolType CurrentToolType { get; private set; }
        
        [SerializeField] private ToolListData _toolListData;
        [SerializeField] private Image[] _toolSlots;
        [SerializeField] private List<GameObject> _toolGameObjects;
    
        private List<ToolData> _tools;
        private List<ToolType> _toolTypes;
        private int _currentToolIndex = 0;
        
        private void Start()
        {
            _tools = _toolListData != null ? _toolListData.ToolList : new List<ToolData>();
            _toolTypes = new List<ToolType> { ToolType.Pickaxe, ToolType.Machete, ToolType.Septer };
    
            if (_tools.Count == 0 || _toolGameObjects.Count == 0 || _tools.Count != _toolGameObjects.Count)
            {
                Debug.LogError("Mismatch entre ToolData et GameObjects !");
                return;
            }
    
            CurrentToolType = _toolTypes[_currentToolIndex];
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
    
            CurrentToolType = _toolTypes[_currentToolIndex];
    
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
}