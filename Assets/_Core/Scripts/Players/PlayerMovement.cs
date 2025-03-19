using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public event Action<float> OnStaminaChanged;

        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private float _sprintSpeed = 4;
        [SerializeField] private float _maxStamina = 100;
        [SerializeField] private bool _canSprint = false;

        private CharacterController _characterController;
        
        private Vector3 _velocity;
        
        private Vector2 _movement;
        private Vector2 _targetMovement;
        
        private float _currentSpeed;
        private float _currentStamina;
        private float _meshScale;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _currentSpeed = _walkSpeed;
            _currentStamina = _maxStamina;
            _meshScale = GetComponentInChildren<SkinnedMeshRenderer>().transform.localScale.y;
        }
        
        private void FixedUpdate()
        {
            UpdateStamina(Time.fixedDeltaTime);
            UpdateMovement(Time.fixedDeltaTime);
            UpdateGravity(Time.fixedDeltaTime);
        }
        
        private void UpdateMovement(float deltaTime)
        {
            _movement = Vector2.Lerp(_movement, _targetMovement, deltaTime * 10f);
            Vector3 move = new Vector3(_movement.x, 0, _movement.y);
            _characterController.Move(move * _currentSpeed * deltaTime);
        
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
        
        private void UpdateGravity(float deltaTime)
        {
            Debug.DrawRay(transform.position, -transform.up * (_meshScale + .5f), Color.red);

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _meshScale) && hit.transform == transform)
            {
                _velocity.y = 0;
            }
            else
            {
                _velocity.y += Physics.gravity.y * deltaTime;
                _characterController.Move(_velocity * deltaTime);
            }
        }

        private void UpdateStamina(float deltaTime)
        {
            float oldStamina = _currentStamina;

            if (_currentSpeed == _sprintSpeed && _targetMovement != Vector2.zero)
            {
                _currentStamina = Mathf.Max(0, _currentStamina - deltaTime);
            }
            else if (_currentStamina < _maxStamina)
            {
                _currentStamina = Mathf.Min(_maxStamina, _currentStamina + deltaTime);
            }

            if (_currentStamina <= 0)
            {
                _currentSpeed = _walkSpeed;
            }

            // Notifie seulement si la stamina a changé
            if (Mathf.Abs(oldStamina - _currentStamina) > Mathf.Epsilon)
            {
                OnStaminaChanged?.Invoke(_currentStamina / _maxStamina);
            }
        }

        public void SetTargetMovement(InputAction.CallbackContext ctx)
        {
            _targetMovement = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
        }
        
        public void SetSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.started && _currentStamina > 0 && _canSprint)
            {
                _currentSpeed = _sprintSpeed;
            }
            else if (ctx.canceled)
            {
                _currentSpeed = _walkSpeed;
            }
        }
    }
}