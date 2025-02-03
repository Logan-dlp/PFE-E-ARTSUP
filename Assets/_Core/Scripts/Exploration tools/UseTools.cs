using UnityEngine;
using UnityEngine.InputSystem;

public class UseTools : MonoBehaviour
{
    private ToolType _currentTool;
    private RouletteSelectionTools _rouletteSelection;

    private void Start()
    {
        _rouletteSelection = FindObjectOfType<RouletteSelectionTools>();
        if (_rouletteSelection == null)
        {
            Debug.LogError("❌ Aucun RouletteSelectionTools trouvé dans la scène !");
        }
    }

    public void UseTool(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _currentTool = _rouletteSelection.GetCurrentToolType();

            switch (_currentTool)
            {
                case ToolType.Machete:
                    UseMachete();
                    break;
                case ToolType.Pickaxe:
                    UsePickaxe();
                    break;
                case ToolType.Septer:
                    UseSepter();
                    break;
            }
        }
    }

    private void UseMachete()
    {
        Debug.Log("🗡️ Coup de machette !");
    }

    private void UsePickaxe()
    {
        Debug.Log("⛏️ Coup de pioche !");
    }

    private void UseSepter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("🔥 Le player a touché un ennemi !");
                Destroy(hit.collider.gameObject);
            }
        }
    }
}