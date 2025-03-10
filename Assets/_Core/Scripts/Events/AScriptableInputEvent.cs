using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Events
{
    public abstract class AScriptableInputEvent : ScriptableObject
    {
        public event Action<InputAction.CallbackContext> OnInput;

        public virtual void SendInput(InputAction.CallbackContext context)
        {
            OnInput?.Invoke(context);
        }
    }
}