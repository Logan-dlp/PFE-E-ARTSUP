using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public event Action<float> OnStaminaChanged;
        public event Action OnFootstep;
        public event Action OnLowStamina;
        public event Action OnStaminaRecovered;

        private bool _lowStaminaTriggered;

        private Vector2 _targetMovement;
        public Vector2 TargetMovement => _targetMovement;

        [SerializeField] private float _walkSpeed = 2;
        public float WalkSpeed => _walkSpeed;

        [SerializeField] private float _sprintSpeed = 4;
        [SerializeField] private float _maxStamina = 100;
        [SerializeField] private float _lowStaminaThreshold = 0.2f;
        [SerializeField] private bool _canSprint = false;
        [SerializeField] private float _floorDistance;
        [SerializeField, MaxValue(0)] private float _maxDownVelocity;

        [SerializeField, Tooltip("Multiplier for how fast footstep sounds play relative to player speed")]
        private float footstepRate = 1.5f;

        private CharacterController _characterController;

        private Vector3 _knockbackMovement = Vector3.zero;
        private Vector3 _velocity;

        private Vector2 _movement;

        private float _currentSpeed;
        public float CurrentSpeed => _currentSpeed;

        private float _currentStamina;
        private bool _isMovementBlocked = false;
        private float _distanceSinceLastFootstep = 0f;

        private bool _isMoving = false;
        private float _footstepTimer = 0f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _currentSpeed = _walkSpeed;
            _currentStamina = _maxStamina;
        }

        private void FixedUpdate()
        {
            if (!_isMovementBlocked)
            {
                UpdateStamina(Time.fixedDeltaTime);
                UpdateMovement(Time.fixedDeltaTime);
                UpdateGravity(Time.fixedDeltaTime);
            }
        }

        private void UpdateMovement(float deltaTime)
        {
            _movement = Vector2.Lerp(_movement, _targetMovement, deltaTime * 10f);
            Vector3 move = new Vector3(_movement.x, 0, _movement.y);
            Vector3 worldMove = move * _currentSpeed * deltaTime;

            _characterController.Move(worldMove);

            float moveMagnitude = move.magnitude;
            float minMoveThreshold = 0.01f;

            if (moveMagnitude > minMoveThreshold)
            {
                gameObject.transform.forward = move;
                _isMoving = true;

                _footstepTimer += deltaTime;

                float stepInterval = 1f / footstepRate;

                if (_footstepTimer >= stepInterval)
                {
                    _footstepTimer = 0f;
                    OnFootstep?.Invoke();
                }
            }
            else
            {
                _isMoving = false;
                _footstepTimer = 0f;
            }
        }

        private void UpdateGravity(float deltaTime)
        {
            Debug.DrawRay(transform.position, -transform.up * _floorDistance, Color.red);

            if (Physics.Raycast(transform.position, -transform.up, _floorDistance))
            {
                _velocity.y = -2;
            }
            else
            {
                _velocity.y = Mathf.Max(_velocity.y + Physics.gravity.y * deltaTime, _maxDownVelocity);
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

            if (Mathf.Abs(oldStamina - _currentStamina) > Mathf.Epsilon)
            {
                float staminaRatio = _currentStamina / _maxStamina;
                OnStaminaChanged?.Invoke(staminaRatio);

                if (!_lowStaminaTriggered && staminaRatio <= _lowStaminaThreshold)
                {
                    _lowStaminaTriggered = true;
                    OnLowStamina?.Invoke();
                }
                else if (_lowStaminaTriggered && staminaRatio > _lowStaminaThreshold)
                {
                    _lowStaminaTriggered = false;
                    OnStaminaRecovered?.Invoke();
                }
            }
        }

        public void SetTargetMovement(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _targetMovement = ctx.ReadValue<Vector2>();

                if (_targetMovement != Vector2.zero && !_isMoving)
                {
                    _footstepTimer = 0f;
                    OnFootstep?.Invoke();
                }
            }
            else
            {
                _targetMovement = Vector2.zero;
            }
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

        public void BlockMovement(bool block)
        {
            _isMovementBlocked = block;
        }

        public IEnumerator Knockback(Vector3 direction, float force, float duration)
        {
            float startTime = Time.time;
            while (Time.time < (startTime + duration))
            {
                _knockbackMovement = Vector3.Lerp(_knockbackMovement, direction, Time.deltaTime * 10f);
                _characterController.Move(_knockbackMovement * force * Time.deltaTime);
                yield return null;
            }
        }
    }
}