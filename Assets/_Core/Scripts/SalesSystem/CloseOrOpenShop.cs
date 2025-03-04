using UnityEngine;
using UnityEngine.InputSystem;
using System;
using MoonlitMixes.AI;

public class CloseOrOpenShop : MonoBehaviour
{
    [SerializeField] private GameObject _openText;
    [SerializeField] private GameObject _closeText;
    [SerializeField] private PNJStateMachine _pnjStateMachine;

    private bool _isPlayerInTrigger = false;
    private bool _isShopOpen = false;

    public static event Action<bool> OnShopToggled;

    private void Start()
    {
        _isShopOpen = false;
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInTrigger = false;
        }
    }

    public void OnToggleShop(InputAction.CallbackContext context)
    {
        if (context.performed && _isPlayerInTrigger)
        {
            _isShopOpen = !_isShopOpen;
            UpdateUI();
            OnShopToggled?.Invoke(_isShopOpen);
        }
    }

    private void UpdateUI()
    {
        _openText.SetActive(_isShopOpen);
        _closeText.SetActive(!_isShopOpen);
    }
}