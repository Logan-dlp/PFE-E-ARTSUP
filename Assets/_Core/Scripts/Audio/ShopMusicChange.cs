using UnityEngine;
using MoonlitMixes.AI.PNJ;

public class ShopMusicChange : MonoBehaviour
{
    [Header("Shop Music")]
    [SerializeField] private AudioEventScriptableObject _shopMusicEvent;
    [Range(0f, 1f)][SerializeField] private float _shopMusicVolume = 1f;

    [Header("Default Music")]
    [SerializeField] private AudioEventScriptableObject _defaultMusicEvent;
    [Range(0f, 1f)][SerializeField] private float _defaultMusicVolume = 1f;

    private void OnEnable()
    {
        CloseOrOpenShop.OnShopToggled += HandleShopToggled;
    }

    private void OnDisable()
    {
        CloseOrOpenShop.OnShopToggled -= HandleShopToggled;
    }

    private void HandleShopToggled(bool isShopOpen)
    {
        if (AudioManager.Instance == null) return;

        if (isShopOpen && _shopMusicEvent != null)
        {
            AudioManager.Instance.PlayPersistentMusic(_shopMusicEvent, _shopMusicVolume);
        }
        else if (!isShopOpen && _defaultMusicEvent != null)
        {
            AudioManager.Instance.PlayPersistentMusic(_defaultMusicEvent, _defaultMusicVolume);
        }
    }
}
