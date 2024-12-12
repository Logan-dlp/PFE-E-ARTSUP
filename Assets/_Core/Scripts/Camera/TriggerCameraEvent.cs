using Unity.Cinemachine;
using UnityEngine;

namespace MoonlitMixes.Camera.Event
{
    public class TriggerCameraEvent : MonoBehaviour
    {
        private enum CameraEvent
        {
            None,
            Zoom,
            Transition,
        }
    
        [SerializeField] private CameraEvent _cameraEvent;
        [SerializeField] private CinemachineCamera _cameraToTransition;
        [SerializeField] private float _zoomTarget;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent<CameraController>(out CameraController cameraController))
            {
                switch (_cameraEvent)
                {
                    case CameraEvent.Zoom:
                        CameraZoom(cameraController, true);
                        break;
                    case CameraEvent.Transition:
                        TransitionToWithPriority(cameraController, 10, 11);
                        break;
                }
            }
        }
    
        private void OnTriggerExit(Collider collider)
        {
            if (collider.TryGetComponent<CameraController>(out CameraController cameraController))
            {
                switch (_cameraEvent)
                {
                    case CameraEvent.Zoom:
                        CameraZoom(cameraController, false);
                        break;
                    case CameraEvent.Transition:
                        TransitionToWithPriority(cameraController, 11, 10);
                        break;
                }
            }
        }
    
        private void CameraZoom(CameraController baseCameraController, bool activeZoom)
        {
            baseCameraController.BaseFollowZoom.Width = activeZoom ? _zoomTarget : baseCameraController.BaseZoom;
        }
    
        private void TransitionToWithPriority(CameraController baseCameraController, int basePriority, int cameraPriority)
        {
            baseCameraController.BaseCamera.Priority = basePriority;
            _cameraToTransition.Priority = cameraPriority;
        }
    }
}