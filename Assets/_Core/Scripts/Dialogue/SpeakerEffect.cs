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
        private bool _isDimmed = false;


        private void Awake()
        {
            _originalScale = transform.localScale;
            _image = GetComponent<Image>();
            _linkedText = GetComponent<TMP_Text>();

            if (_image != null)
            {
                _originalColor = _image.color;
            }
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

                case SpeakerEffectType.FadeIn:
                    StartCoroutine(FadeInEffect());
                    break;

                case SpeakerEffectType.FadeOut:
                    StartCoroutine(FadeOutEffect());
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
            _isDimmed = true;

            if (_image != null)
            {
                _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f); // Sprite semi-transparent

                RectTransform rectTransform = _image.rectTransform;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);
            }

            if (_linkedText != null)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 0.5f); // Texte semi-transparent
            }
        }

        private IEnumerator FadeInEffect()
        {
            _isDimmed = false;

            if (_image)
            {
                _image.color = _originalColor;
                RectTransform rectTransform = _image.rectTransform;
                rectTransform.sizeDelta = new Vector2(_originalScale.x * 300f, _originalScale.y * 300f);
            }

            if (_linkedText)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);
            }

            float duration = _dialogueLineData != null ? _dialogueLineData.FadeInDuration : 0.5f;
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;
                float alphaImage = Mathf.Lerp(0f, 1f, t);
                float alphaText = Mathf.Lerp(0f, 1f, t);

                if (_image)
                {
                    var color = _image.color;
                    _image.color = new Color(color.r, color.g, color.b, alphaImage);
                }

                if (_linkedText)
                {
                    var color = _linkedText.color;
                    _linkedText.color = new Color(color.r, color.g, color.b, alphaText);
                }

                time += Time.deltaTime;
                yield return null;
            }

            if (_image)
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);

            if (_linkedText)
                _linkedText.color = new Color(_linkedText.color.r, _linkedText.color.g, _linkedText.color.b, 1f);
        }

        private IEnumerator FadeOutEffect()
        {
            _isDimmed = false;

            float duration = _dialogueLineData != null ? _dialogueLineData.FadeOutDuration : 0.5f;
            float time = 0f;

            float startAlphaImage = _image ? _image.color.a : 1f;
            float startAlphaText = _linkedText ? _linkedText.color.a : 1f;

            while (time < duration)
            {
                float t = time / duration;
                float alphaImage = Mathf.Lerp(startAlphaImage, 0f, t);
                float alphaText = Mathf.Lerp(startAlphaText, 0f, t);

                if (_image)
                {
                    var color = _image.color;
                    _image.color = new Color(color.r, color.g, color.b, alphaImage);
                }

                if (_linkedText)
                {
                    var color = _linkedText.color;
                    _linkedText.color = new Color(color.r, color.g, color.b, alphaText);
                }

                time += Time.deltaTime;
                yield return null;
            }

            if (_image)
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);

            if (_linkedText)
                _linkedText.color = new Color(_linkedText.color.r, _linkedText.color.g, _linkedText.color.b, 0f);
        }

        public void ResetEffect()
        {
            if (_image != null)
            {
                _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1f); // Sprite opaque

                RectTransform rectTransform = _image.rectTransform;
                rectTransform.sizeDelta = new Vector2(_originalScale.x * 300f, _originalScale.y * 300f);
            }

            if (_linkedText != null)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 1f); // Texte opaque
            }
        }
    }
}