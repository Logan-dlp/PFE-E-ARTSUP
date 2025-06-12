using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.CookingMachine;

namespace MoonlitMixes.Potion
{
    public class CauldronTimer : MonoBehaviour
    {
        public bool PotionSuccessExpected { get; set; }

        [Header("Cooldown Duration")]
        [SerializeField] private float _itemCooldown = 60f;

        [Header("Images Of Progress Bar")]
        [SerializeField] private Image _fillBarFront;
        [SerializeField] private Image _fillBarBack;

        private float _fillBarFrontValue;
        private float _fillBarBackValue;
        private float _remainingTime;

        private CauldronVFXController _cauldronVFXController;
        private CauldronRecipeChecker _cauldronRecipeChecker;
        private bool _timerFinished;
        private bool _timerIsActive;
        private bool _canAction = true;
        private bool _isBubbleVFXUp;
        private bool _isSmokeVFXUp;
        private bool _isBurnVFXUp;
        private bool _isFireVFXUp;
        public bool WaitingForNextIngredient = false;

        public bool CanAction
        {
            get => _canAction;
            set => _canAction = value;
        }

        public bool TimerIsActive
        {
            get => _timerIsActive;
            set => _timerIsActive = value;
        }

        public float RemainingTime
        {
            get => _remainingTime;
        }

        private void Awake()
        {
            _cauldronRecipeChecker = GetComponent<CauldronRecipeChecker>();
            _cauldronVFXController = GetComponent<CauldronVFXController>();
        }

        private void FixedUpdate()
        {
            if (!_timerIsActive) return;

            if (!WaitingForNextIngredient)
            {
                // Phase 1 : validation de l'item ajouté (première moitié du timer)
                if (_remainingTime >= _itemCooldown / 2)
                {
                    _remainingTime -= Time.fixedDeltaTime;
                    _fillBarFrontValue = _remainingTime / _itemCooldown * 2;
                    _fillBarFront.fillAmount = _fillBarFrontValue - 1;
                    _canAction = false;

                    if (!_isBubbleVFXUp)
                    {
                        //_cauldronVFXController.PlayBubble();
                    }
                    if (!_isFireVFXUp)
                    {
                        //_cauldronVFXController.PlayFire();
                    }
                }
                else
                {
                    if (PotionSuccessExpected)
                    {
                        _cauldronRecipeChecker.ValidateIngredientAfterTimer();
                        PotionSuccessExpected = false;
                    }

                    // Fin phase 1 → passer à la phase 2 (attente ingrédient)
                    WaitingForNextIngredient = true;
                    _remainingTime = _itemCooldown / 2;
                    _canAction = true;

                    if (_cauldronRecipeChecker != null)
                        _cauldronRecipeChecker.NeedItem = true;
                }
            }
            else
            {
                // Phase 2 : attente d'un nouvel ingrédient (seconde moitié)
                if (_remainingTime > 0)
                {
                    _remainingTime -= Time.fixedDeltaTime;
                    _fillBarBackValue = _remainingTime / (_itemCooldown / 2);
                    _fillBarBack.fillAmount = _fillBarBackValue;

                    if (!_isSmokeVFXUp && !PotionSuccessExpected)
                    {
                        _cauldronVFXController.PlaySmoke();
                        _isSmokeVFXUp = true;
                    }
                }
                else
                {
                    // Timer fini en phase d'attente => potion ratée
                    _timerIsActive = false;
                    _timerFinished = true;

                    if (_cauldronRecipeChecker.QteInProgress)
                    {
                        _cauldronRecipeChecker.CheckQTE(false);
                    }
                    else
                    {
                        _cauldronRecipeChecker.HandleFailedPotion();
                    }

                    if (!_isBurnVFXUp && !PotionSuccessExpected)
                    {
                        _cauldronVFXController.PlayBurned();
                        _isBurnVFXUp = true;
                    }
                }
            }
        }

        public void StartCooldown()
        {
            _remainingTime = _itemCooldown;
            _fillBarBack.fillAmount = 1f;
            _fillBarFront.fillAmount = 1f;
            _fillBarFrontValue = _remainingTime * 0.5f;
            _fillBarBackValue = _remainingTime * 0.5f;

            _isSmokeVFXUp = false;
            _isBurnVFXUp = false;
            _isBubbleVFXUp = true;
            _isFireVFXUp = false;

            _timerIsActive = true;
            _canAction = false;
        }

        public void ResetCooldown()
        {
            _remainingTime = _itemCooldown;
            TimerIsActive = true;
            CanAction = false;
            _timerFinished = false;
            _fillBarBack.fillAmount = 1;
            _fillBarFront.fillAmount = 1;
            _fillBarFrontValue = _remainingTime * .5f;
            _fillBarBackValue = _remainingTime * .5f;
            _isSmokeVFXUp = false;
            _isBurnVFXUp = false;
            _isBubbleVFXUp = false;
        }

        public void StopCooldown()
        {
            _remainingTime = _itemCooldown;
            _timerIsActive = false;
            _fillBarBack.fillAmount = 0;
            _fillBarFront.fillAmount = 0;
            _canAction = true;

            _isFireVFXUp = false;
            _isBubbleVFXUp = false;
            _isSmokeVFXUp = false;
            _isBurnVFXUp = true;
            _cauldronVFXController.StopFire();
        }
    }
}