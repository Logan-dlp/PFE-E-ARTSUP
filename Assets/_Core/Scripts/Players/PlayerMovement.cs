using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private float _sprintSpeed = 4;
        [SerializeField] private float _maxStamina = 100;
        
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
            _meshScale = GetComponentInChildren<MeshRenderer>().transform.localScale.y;
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
            Debug.DrawRay(transform.position, Vector3.down * 0.76f, Color.red);

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _meshScale + .5f))
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
            if (_currentSpeed == _sprintSpeed && _targetMovement != Vector2.zero)
            {
                _currentStamina -= deltaTime;
            }
            else if (_currentStamina < _maxStamina)
            {
                _currentStamina += deltaTime;
            }
        
            if (_currentStamina <= 0)
            {
                _currentSpeed = _walkSpeed;
            }
        }
        
        public void SetTargetMovement(InputAction.CallbackContext ctx)
        {
            _targetMovement = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
        }
        
        public void SetSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.started && _currentStamina > 0)
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