using UnityEngine;

public class RockHealth : MonoBehaviour
{
    private int _hitsRemaining = 3;

    public bool TakeDamage()
    {
        if (_hitsRemaining <= 0) return false;  // Si la pierre est d�j� d�truite, rien ne se passe.

        _hitsRemaining--;

        if (_hitsRemaining <= 0)
        {
            Destroy(gameObject);  // Au cas o� si plus tard on change le gameObject
        }

        return true;
    }
}