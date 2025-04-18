﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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
        private Animator _animator;

        private Vector3 _knockbackMovement = Vector3.zero;
        private Vector3 _velocity;
        private Vector2 _movement;
        private Vector2 _targetMovement;

        private float _currentSpeed;
        private float _currentStamina;
        private float _meshScale;
        private bool _isInventoryOpen = false;
        private bool _isPerformingActionIdle = true;
        private bool _isPerformingActionHolding = false;
        private bool _isCut = false;
        private bool _isCrush = false;
        private bool _isMix = false;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _currentSpeed = _walkSpeed;
            _currentStamina = _maxStamina;
            _meshScale = GetComponentInChildren<SkinnedMeshRenderer>().transform.localScale.y;
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            UpdateStamina(Time.fixedDeltaTime);
            UpdateMovement(Time.fixedDeltaTime);
            UpdateGravity(Time.fixedDeltaTime);
            UpdateAnimations();
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

            if (Mathf.Abs(oldStamina - _currentStamina) > Mathf.Epsilon)
            {
                OnStaminaChanged?.Invoke(_currentStamina / _maxStamina);
            }
        }

        private void UpdateAnimations()
        {
            bool isMoving = _targetMovement.magnitude > 0.1f;
            bool isHoldingItem = GetComponent<PlayerHoldItem>().ItemHold != null;

            _isPerformingActionHolding = isHoldingItem && !_isCut && !_isMix && !_isCrush;

            _animator.SetBool("isHoldingIdle", _isPerformingActionHolding && !isMoving);
            _animator.SetBool("isHoldingRun", _isPerformingActionHolding && isMoving);

            if (!_isPerformingActionHolding)
            {
                if (_isPerformingActionIdle && !_isMix && !_isInventoryOpen)
                {
                    _animator.SetBool("isRun", isMoving);
                    _animator.SetBool("isIdle", !isMoving);
                }
                else if (_isMix)
                {
                    _isPerformingActionIdle = false;
                    _animator.SetBool("isIdle", false);
                    _animator.SetBool("isRun", false);
                }
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

        public void OpenInventory()
        {
            _isInventoryOpen = true;
            _animator.SetBool("isLongIdle", true);
        }

        public void CloseInventory()
        {
            _isInventoryOpen = false;
            _animator.SetBool("isLongIdle", false);
        }

        public void InteractCut()
        {
            _isCut = true;
            _animator.SetBool("isCut", true);
        }

        public void FinishedInteractCut()
        {
            _isCut = false;
            _animator.SetBool("isCut", false);
        }

        public void InteractMix()
        {
            _isMix = true;
            _animator.SetBool("isMix", true);
            _isPerformingActionHolding = false;
            _animator.SetBool("isHoldingRun", false);
        }

        public void FinishedInteractMix()
        {
            _isMix = false;
            _animator.SetBool("isMix", false);
        }

        public void InteractCrush()
        {
            _isCrush = true;
            _animator.SetBool("isCrush", true);
        }

        public void FinishedInteractCrush()
        {
            _isCrush = false;
            _animator.SetBool("isCrush", false);
        }

        public void SetPerformingActionHolding(bool state)
        {
            _isPerformingActionHolding = state;
            _isPerformingActionIdle = !state;

            _animator.SetBool("isIdle", false);
            _animator.SetBool("isHoldingIdle", false);
            _animator.SetBool("isRun", false);
            _animator.SetBool("isHoldingRun", false);

            if (state)
            {
                _animator.SetBool("isHoldingIdle", true);
                _animator.SetBool("isHoldingRun", true);
            }
        }

        public void SetPerformingActionIdle(bool state)
        {
            _isPerformingActionIdle = state;
            _isPerformingActionHolding = !state;

            _animator.SetBool("isHoldingIdle", false);
            _animator.SetBool("isHoldingRun", false);
            _animator.SetBool("isRun", false);

            if (state)
            {
                _animator.SetBool("isIdle", true);
            }
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