using UnityEngine;

public class PickUpTool : MonoBehaviour
{
    [SerializeField] private ToolData _toolData;
    [SerializeField] private GameObject _toolPrefab;
    [SerializeField] private RouletteSelectionTools _rouletteSelectionTools;

    private bool _isToolPickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isToolPickedUp) return;

        if (other.CompareTag("Player"))
        {
            PickupTool();
            gameObject.SetActive(false);
            _isToolPickedUp = true;
        }
    }

    private void PickupTool()
    {
        if (_rouletteSelectionTools != null)
        {
            _rouletteSelectionTools.AddTool(_toolData, _toolPrefab);
        }
    }
}