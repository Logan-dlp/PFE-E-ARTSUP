using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.ExplorationTools
{
    public class PickUpTool : MonoBehaviour
    {
        [SerializeField] private ToolData _toolData;
        [SerializeField] private GameObject _toolPrefab;
        [SerializeField] private RouletteSelectionTools _rouletteSelectionTools;
        [SerializeField] private ToolAcquired toolAcquired;

        private ToolAcquired.Tool tool;

        private bool _canToolPickedUp = false;

        public bool canToolPickedUp
        {
            set => _canToolPickedUp = value;
        }

        private void Start()
        {
            tool.toolData = _toolData;
            tool.toolPrefab = _toolPrefab;

            if (toolAcquired.toolAcquiredArray.Contains(tool))
            {
                Debug.Log("");
                gameObject.SetActive(false);
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canToolPickedUp) return;

            if (other.CompareTag("Player"))
            {
                PickupTool();
                gameObject.SetActive(false);
            }
        }

        private void PickupTool()
        {
            toolAcquired.toolAcquiredArray.Add(tool);

            if (_rouletteSelectionTools != null)
            {
                _rouletteSelectionTools.AddTool(_toolData, _toolPrefab);
            }
        }
    }
}