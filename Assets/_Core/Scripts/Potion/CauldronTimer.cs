using UnityEngine;

public class CauldronTimer : MonoBehaviour
{
    [SerializeField] private float _itemCooldown = 60f;
    private float _lastItemTime = -60f;

    public bool CanAddItem()
    {
        return Time.time >= _lastItemTime + _itemCooldown;
    }

    public void ResetCooldown()
    {
        _lastItemTime = Time.time;
    }

    public float GetTimeRemaining()
    {
        return Mathf.Max(0, _lastItemTime + _itemCooldown - Time.time);
    }
}
