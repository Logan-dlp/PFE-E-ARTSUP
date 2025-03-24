using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Inputs
{
    public class InputManager : MonoBehaviour
    {
        // Make Singleton !
        private InputManager _instance;
        public InputManager Instance => _instance;
        
        private PlayerInput _currentPlayerInput;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            
            // Change current player input for change scenes
            _instance._currentPlayerInput = FindFirstObjectByType<PlayerInput>();
        }

        public void SwitchActionMap(string mappingName)
        {
            if (_currentPlayerInput.actions.FindActionMap(mappingName) != null)
            {
                _currentPlayerInput.SwitchCurrentActionMap(mappingName);
            }
            else
            {
                Debug.LogError($"No mapping found for {mappingName}.");
            }
        }
    }
}