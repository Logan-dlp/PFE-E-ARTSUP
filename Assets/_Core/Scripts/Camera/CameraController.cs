using Unity.Cinemachine;
using UnityEngine;

namespace MoonlitMixes.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _baseCamera;
        public CinemachineCamera BaseCamera => _baseCamera;

        private CinemachineFollowZoom _baseFollowZoom;
        public CinemachineFollowZoom BaseFollowZoom => _baseFollowZoom;
    
        public float BaseZoom { get; private set; }

        private void Awake()
        {
            if (_baseCamera.TryGetComponent(out CinemachineFollowZoom followZoom))
            {
                _baseFollowZoom = followZoom;
                BaseZoom = _baseFollowZoom.Width;
            }

        }
    }
}