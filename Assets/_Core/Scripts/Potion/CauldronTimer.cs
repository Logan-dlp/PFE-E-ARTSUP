using UnityEngine;

public class CauldronTimer : MonoBehaviour
{
    [SerializeField] private float _itemCooldown = 60f;
    private float _lastItemTime = -60f;

    private void Update()
    {
        float timeRemaining = GetTimeRemaining();

        if (timeRemaining > 0)
        {
            Debug.Log($"Cooldown restant: {timeRemaining:F2} secondes");
        }
        else
        {
            Debug.Log("Le cooldown est terminé. Vous pouvez ajouter un nouvel élément !");
        }
    }

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