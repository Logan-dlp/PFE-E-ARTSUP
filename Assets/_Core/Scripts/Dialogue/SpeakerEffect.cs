using System.Collections;
using UnityEngine;

public class SpeakerEffect : MonoBehaviour
{
    private Vector3 _originalScale;
    private Color _originalColor;

    private void Start()
    {
        _originalScale = transform.localScale;
        _originalColor = GetComponent<SpriteRenderer>().color;
    }

    public void ApplyEffect(SpeakerEffectType effect)
    {
        switch (effect)
        {
            case SpeakerEffectType.Tremble:
                StartCoroutine(TrembleEffect());
                break;
            case SpeakerEffectType.Jump:
                StartCoroutine(JumpEffect());
                break;
            default:
                break;
        }
    }

    // Effet de tremblement
    private IEnumerator TrembleEffect()
    {
        Vector3 originalPos = transform.position;
        for (int i = 0; i < 5; i++)
        {
            transform.position = originalPos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = originalPos;
    }

    // Effet de saut
    private IEnumerator JumpEffect()
    {
        Vector3 originalPos = transform.position;
        for (int i = 0; i < 5; i++)
        {
            transform.position = originalPos + new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(0.1f);
            transform.position = originalPos;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Appliquer l'effet de "disparition" (le personnage devient plus petit et sombre)
    public void DimEffect()
    {
        transform.localScale = _originalScale * 0.8f;  // Réduit la taille
        GetComponent<SpriteRenderer>().color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f);  // Dim
    }

    // Restaurer l'état normal
    public void ResetEffect()
    {
        transform.localScale = _originalScale;
        GetComponent<SpriteRenderer>().color = _originalColor;
    }
}
