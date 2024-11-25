using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speedMovement = 2;
    
    private CharacterController _characterController;
    
    private Vector3 _velocity;
    private Vector2 _movement;
    private Vector2 _targetMovement;
    
    private const float _gravityValue = -9.81f;
    private bool _isGrounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        UpdateMovement(Time.fixedDeltaTime);
        UpdateGravity(Time.fixedDeltaTime);
    }

    private void UpdateMovement(float deltaTime)
    {
        _movement = Vector2.Lerp(_movement, _targetMovement, deltaTime * 10f);
        Vector3 move = new Vector3(_movement.x, 0, _movement.y);
        _characterController.Move(move * _speedMovement * deltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }

    private void UpdateGravity(float deltaTime)
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _characterController.velocity.y < 0)
        {
            _velocity.y = 0;
        }
        
        _velocity.y += _gravityValue * deltaTime;
        _characterController.Move(_velocity * deltaTime);
    }

    public void SetTargetMovement(InputAction.CallbackContext ctx)
    {
        _targetMovement = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
    }
    
}
