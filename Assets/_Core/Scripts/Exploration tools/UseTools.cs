using UnityEngine;
using UnityEngine.InputSystem;

public class UseTools : MonoBehaviour
{
    public void UseTool(InputAction.CallbackContext ctx)
    {
        Debug.Log("Tu utilises le Tool !");
    }
}