using MoonlitMixes.CookingMachine;
using MoonlitMixes.Potion;
using Unity.Cinemachine;
using UnityEngine;

namespace MoonlitMixes.Camera.Event
{
    public class StirCameraTransition : MonoBehaviour
    {
        [Header("Cauldron Reference")]
        [SerializeField] private CauldronRecipeChecker _cauldronRecipeChecker;

        [Header("Camera Settings")]
        [SerializeField] private CinemachineCamera _baseCameraController;

        [SerializeField] private int _basePriority = 10;
        [SerializeField] private int _stirPriority = 11;

        private CinemachineCamera _stirCamera;

        private void Awake()
        {
            _stirCamera = GetComponent<CinemachineCamera>();

            if (_cauldronRecipeChecker == null || _stirCamera == null || _baseCameraController == null)
            {
                Debug.LogError("Missing reference in StirCameraTransition.");
                enabled = false;
                return;
            }

            _cauldronRecipeChecker.OnStirStarted += HandleStirStart;
            _cauldronRecipeChecker.OnStirEnded += HandleStirEnd;
        }

        private void HandleStirStart()
        {
            _baseCameraController.Priority = _stirPriority;
            _stirCamera.Priority = _basePriority;
        }

        private void HandleStirEnd()
        {
            _baseCameraController.Priority = _basePriority;
            _stirCamera.Priority = _stirPriority;
        }

        private void OnDestroy()
        {
            _cauldronRecipeChecker.OnStirStarted -= HandleStirStart;
            _cauldronRecipeChecker.OnStirEnded -= HandleStirEnd;
        }
    }
}