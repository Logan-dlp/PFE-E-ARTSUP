using MoonlitMixes.Datas;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Dialogue.Effect
{
    public class SpeakerEffect : MonoBehaviour
    {
        private Vector3 _originalScale;
        private Color _originalColor;
        private Image _image;
        private TMP_Text _linkedText;

        private DialogueLineData _dialogueLineData;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _image = GetComponent<Image>();
            _linkedText = GetComponent<TMP_Text>();

            if (_image != null)
            {
                _originalColor = _image.color;
            }

            Debug.Log($"Image is null: {_image == null}, TMP_Text is null: {_linkedText == null}");
        }

        public void SetDialogueLineData(DialogueLineData dialogueLineData)
        {
            _dialogueLineData = dialogueLineData;
        }

        public void SetLinkedText(TMP_Text text)
        {
            _linkedText = text;
        }

        public void ApplyEffect(SpeakerEffectType effectType)
        {
            switch (effectType)
            {
                case SpeakerEffectType.Tremble:
                    StartCoroutine(TrembleEffect());
                    break;

                case SpeakerEffectType.Jump:
                    StartCoroutine(JumpEffect());
                    break;

                default:
                    Debug.LogWarning("Unknown effect type");
                    break;
            }
        }

        private IEnumerator TrembleEffect()
        {
            if (_image == null && _linkedText == null || _dialogueLineData == null)
            {
                Debug.LogWarning("No valid image or linkedText for tremble effect.");
                yield break;
            }

            Vector3 originalPosition = transform.localPosition;
            Quaternion originalRotation = transform.rotation;
            Vector3 textOriginalPosition = _linkedText != null ? _linkedText.rectTransform.localPosition : Vector3.zero;

            transform.localScale = new Vector3(1f, 1f, 1f);
            if (_linkedText)
            {
                _linkedText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
            }

            float intensityX = _dialogueLineData.TrembleIntensityX;
            float intensityY = _dialogueLineData.TrembleIntensityY;

            // Début du tremblement
            for (int i = 0; i < 20; i++)
            {
                // Applique une variation aléatoire à la position de l'image
                Vector3 offset = new Vector3(Random.Range(-intensityX, intensityX), Random.Range(-intensityY, intensityY), 0);
                transform.localPosition = originalPosition + offset;

                // Applique un tremblement à la position du texte
                if (_linkedText)
                {
                    RectTransform textRect = _linkedText.rectTransform;
                    Vector3 textOffset = new Vector3(Random.Range(-intensityX, intensityX), Random.Range(-intensityY, intensityY), 0);
                    textRect.localPosition = textOriginalPosition + textOffset;
                }

                // Variation légère de la rotation de l'image pour un effet de tremblement
                float trembleAmount = Random.Range(-5f, 5f);
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, trembleAmount);

                yield return new WaitForSeconds(0.05f);  // Pause avant le prochain tremblement
            }

            transform.localPosition = originalPosition;
            transform.rotation = originalRotation;

            if (_linkedText)
            {
                _linkedText.rectTransform.localPosition = textOriginalPosition;
            }
        }

        private IEnumerator JumpEffect()
        {
            if (_image == null && _linkedText == null)
                yield break;

            Vector3 originalPos = transform.localPosition;
            Vector3 textOriginalPos = _linkedText ? _linkedText.rectTransform.localPosition : Vector3.zero;

            for (int i = 0; i < 10; i++)
            {
                Vector3 jump = new Vector3(0, 15f, 0);  // Saut de l'image
                transform.localPosition = originalPos + jump;

                if (_linkedText)
                    _linkedText.rectTransform.localPosition = textOriginalPos + jump;

                yield return new WaitForSeconds(0.1f);

                transform.localPosition = originalPos;

                if (_linkedText)
                    _linkedText.rectTransform.localPosition = textOriginalPos;

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void DimEffect()
        {
            if (_image != null)
            {
                _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f); // Sprite semi-transparent
            }

            if (_linkedText != null)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 0.5f); // Texte semi-transparent
            }
        }

        public void ResetEffect()
        {
            if (_image != null)
            {
                _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1f); // Sprite opaque
            }

            if (_linkedText != null)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 1f); // Texte opaque
            }
        }
    }
}