using MoonlitMixes.Datas;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Dialogue.Effect
{
    public class SpeakerEffect : MonoBehaviour
    {
        private Vector2 _originalSizeDelta;
        private Color _originalColor;
        private Image _image;
        private TMP_Text _linkedText;

        private DialogueLineData _dialogueLineData;
        private bool _isDimmed = false;

        public bool SkipEffectNow { get; set; } = false;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _linkedText = GetComponent<TMP_Text>();

            if (_image != null)
            {
                _originalColor = _image.color;
                _originalSizeDelta = _image.rectTransform.sizeDelta;
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

        public IEnumerator PlayEffect(SpeakerEffectType effectType)
        {
            gameObject.SetActive(true);
            SkipEffectNow = false;

            switch (effectType)
            {
                case SpeakerEffectType.Tremble:
                    yield return StartCoroutine(TrembleEffect());
                    break;
                case SpeakerEffectType.Jump:
                    yield return StartCoroutine(JumpEffect());
                    break;
                case SpeakerEffectType.FadeIn:
                    yield return StartCoroutine(FadeInEffect());
                    break;
                case SpeakerEffectType.FadeOut:
                    yield return StartCoroutine(FadeOutEffect());
                    break;
                default:
                    yield break;
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

            transform.localScale = Vector3.one;
            if (_linkedText)
                _linkedText.rectTransform.localScale = Vector3.one;

            float intensityX = _dialogueLineData.TrembleIntensityX;
            float intensityY = _dialogueLineData.TrembleIntensityY;

            // Début du tremblement
            for (int i = 0; i < 20; i++)
            {
                if (SkipEffectNow) break;

                // Applique une variation aléatoire à la position de l'image
                Vector3 offset = new Vector3(Random.Range(-intensityX, intensityX), Random.Range(-intensityY, intensityY), 0);
                transform.localPosition = originalPosition + offset;

                // Applique un tremblement à la position du texte
                if (_linkedText)
                {
                    Vector3 textOffset = new Vector3(Random.Range(-intensityX, intensityX), Random.Range(-intensityY, intensityY), 0);
                    _linkedText.rectTransform.localPosition = textOriginalPosition + textOffset;
                }

                // Variation légère de la rotation de l'image pour un effet de tremblement
                transform.rotation = originalRotation * Quaternion.Euler(0, 0, Random.Range(-5f, 5f));

                yield return new WaitForSeconds(0.05f);
            }

            transform.localPosition = originalPosition;
            transform.rotation = originalRotation;

            if (_linkedText)
                _linkedText.rectTransform.localPosition = textOriginalPosition;
        }

        private IEnumerator JumpEffect()
        {
            if (_image == null && _linkedText == null) yield break;

            Vector3 originalPos = transform.localPosition;
            Vector3 textOriginalPos = _linkedText ? _linkedText.rectTransform.localPosition : Vector3.zero;

            for (int i = 0; i < 10; i++)
            {
                if (SkipEffectNow) break;

                Vector3 jump = new Vector3(0, 15f, 0);
                transform.localPosition = originalPos + jump;
                if (_linkedText)
                    _linkedText.rectTransform.localPosition = textOriginalPos + jump;

                yield return new WaitForSeconds(0.1f);

                transform.localPosition = originalPos;
                if (_linkedText)
                    _linkedText.rectTransform.localPosition = textOriginalPos;

                yield return new WaitForSeconds(0.1f);
            }

            transform.localPosition = originalPos;
            if (_linkedText)
                _linkedText.rectTransform.localPosition = textOriginalPos;
        }

        public void DimEffect()
        {
            if (_isDimmed) return;

            _isDimmed = true;

            if (_image != null)
            {
                _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f);

                Vector2 currentSize = _image.rectTransform.sizeDelta;
                _image.rectTransform.sizeDelta = new Vector2(currentSize.x * 0.8f, currentSize.y * 0.8f);
            }

            if (_linkedText != null)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 0.5f);
            }
        }

        private IEnumerator FadeInEffect()
        {
            _isDimmed = false;

            if (_image) _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0f);
            if (_linkedText)
                _linkedText.color = new Color(_linkedText.color.r, _linkedText.color.g, _linkedText.color.b, 0f);

            float duration = _dialogueLineData?.FadeInDuration ?? 0.5f;
            float time = 0f;

            while (time < duration)
            {
                if (SkipEffectNow) break;

                float t = time / duration;
                float alpha = Mathf.Lerp(0f, 1f, t);

                if (_image)
                {
                    var color = _image.color;
                    _image.color = new Color(color.r, color.g, color.b, alpha);
                }

                if (_linkedText)
                {
                    var color = _linkedText.color;
                    _linkedText.color = new Color(color.r, color.g, color.b, alpha);
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

            float duration = _dialogueLineData?.FadeOutDuration ?? 0.5f;
            float time = 0f;

            float startAlphaImage = _image ? _image.color.a : 1f;
            float startAlphaText = _linkedText ? _linkedText.color.a : 1f;

            while (time < duration)
            {
                if (SkipEffectNow) break;

                float t = time / duration;
                float alpha = Mathf.Lerp(startAlphaImage, 0f, t);

                if (_image)
                {
                    var color = _image.color;
                    _image.color = new Color(color.r, color.g, color.b, alpha);
                }

                if (_linkedText)
                {
                    var color = _linkedText.color;
                    _linkedText.color = new Color(color.r, color.g, color.b, alpha);
                }

                time += Time.deltaTime;
                yield return null;
            }
        }

        public void ResetEffect()
        {
            _isDimmed = false;
            SkipEffectNow = false;

            if (_image != null)
            {
                _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1f);
                _image.rectTransform.sizeDelta = _originalSizeDelta;
            }

            if (_linkedText != null)
            {
                var textColor = _linkedText.color;
                _linkedText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
            }
        }
    }
}