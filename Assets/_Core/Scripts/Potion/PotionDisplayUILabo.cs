using MoonlitMixes.Datas;
using MoonlitMixes.Potion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class PotionDisplayUI : MonoBehaviour
    {
        [SerializeField] private PotionListData _potionListData;
        [SerializeField] private GameObject _slotPrefab;

        private int _lastPotionCount = 0;

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            _lastPotionCount = _potionListData.PotionResults.Count;

            foreach (PotionResult potion in _potionListData.PotionResults)
            {
                GameObject newSlot = Instantiate(_slotPrefab, transform);

                Image image = newSlot.GetComponentInChildren<Image>();
                if (image != null && potion.Recipe != null)
                {
                    image.sprite = potion.Recipe.PotionSprite;
                }

                TextMeshProUGUI text = newSlot.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null && potion.Recipe != null)
                {
                    text.text = potion.Recipe.RecipeName;
                }
            }
        }

        private void Update()
        {
            if (_potionListData.PotionResults.Count != _lastPotionCount)
            {
                RefreshUI();
            }
        }
    }
}