using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ScriptableCallbackContextEvent", menuName = "Scriptable Objects/Event/ScriptableCallbackContextEvent")]
public class ScriptableCallbackContextEvent : ScriptableObject
{
    public Action<InputAction.CallbackContext> OnContextEvent;

    public void SendContext(InputAction.CallbackContext callbackContext)
    {
        OnContextEvent?.Invoke(callbackContext);
    }
}
