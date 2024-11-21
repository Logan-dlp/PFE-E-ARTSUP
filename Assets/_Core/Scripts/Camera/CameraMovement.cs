using UnityEngine;

namespace MoonlitMixes.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] GameObject _target;
        [SerializeField, Range(0, 1)] private float _positionCamera;
        [SerializeField] private Transform _further;
        [SerializeField] private Transform _closer;
        [SerializeField] private Transform _caldron;
        [SerializeField] bool _inOutside;

        private float _deltaPosition = 0;

        private void Update()
        {
            if (_inOutside)
            {
                SetCameraDistance(_further.position, _closer.position, _target.transform.position, _target.transform.position, _positionCamera);
            }
            else
            {
                SetCameraDistance(_caldron.position, _closer.position,transform.position - _target.transform.right, _target.transform.position, _positionCamera);
            }
        }

        private void SetCameraDistance(Vector3 a, Vector3 b, Vector3 lookA, Vector3 lookB, float delta)
        {
            void UpdateDeltaPosition()
            {
                if (_deltaPosition == delta)
                {
                    _deltaPosition = delta;
                }
                
                if (_deltaPosition < delta)
                {
                    _deltaPosition += Time.deltaTime;
                }
                
                if (_deltaPosition > delta)
                {
                    _deltaPosition -= Time.deltaTime;
                }
            }
            
            UpdateDeltaPosition();
            transform.position = Vector3.Lerp(a, b, _deltaPosition);
            transform.LookAt(Vector3.Lerp(lookA, lookB, _deltaPosition));
        }
    }
}