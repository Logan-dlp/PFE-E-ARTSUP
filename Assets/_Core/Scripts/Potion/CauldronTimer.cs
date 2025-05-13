using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.CookingMachine;

namespace MoonlitMixes.Potion
{
    public class CauldronTimer : MonoBehaviour
    {
        [Header("Cooldown Durations")]
        [SerializeField] private float _itemCooldown = 60f;

        [Header("Images Of Progress Bar")]
        [SerializeField] private Image _fillBarFront;
        [SerializeField] private Image _fillBarBack;

        [Header("VFX")]
        [SerializeField] private ParticleSystem _bubbleVFX;
        [SerializeField] private ParticleSystem _smokeVFX;
        [SerializeField] private ParticleSystem _burnedVFX;
        [SerializeField] private ParticleSystem[] _fireVFXArray;
    
        private float _fillBarFrontValue;
        private float _fillBarBackValue;
        private float _remainingTime;
        
        private CauldronRecipeChecker _cauldronRecipeChecker;
        private bool _timerFinished;
        private bool _timerIsActive;
        private bool _canAction = true;
        private bool _isSmokeVFXUp;
        private bool _isBurnVFXUp;
        private bool _isBubbleVFXUp;
        private bool _isFireVFXUp;
        
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
        }
    
        private void FixedUpdate()
        {        
            if(!_timerIsActive) return;
            
            if (_remainingTime >= _itemCooldown/2)
            {
                _remainingTime -= Time.fixedDeltaTime;
                _fillBarFrontValue = _remainingTime / _itemCooldown * 2;
                _fillBarFront.fillAmount = _fillBarFrontValue - 1;
                _canAction = false;
                
                if(!_isBubbleVFXUp)
                {
                    _bubbleVFX.Play();
                    _isBubbleVFXUp = true;
                }
                if(!_isFireVFXUp)
                {
                    foreach(ParticleSystem fireVFX in _fireVFXArray)
                    {
                        fireVFX.Play();
                    }
                    _isFireVFXUp = true;
                }
                //Debug.Log($"Cooldown restant: {_remainingTime:F2} secondes");
            }
            else if (_remainingTime >= 0)
            {   
                _remainingTime -= Time.fixedDeltaTime;
                _canAction = true;
                _fillBarBackValue = _remainingTime / _itemCooldown * 2;
                _fillBarBack.fillAmount = _fillBarBackValue;
                
                if(!_isSmokeVFXUp)
                {
                    _smokeVFX.Play();
                    _isSmokeVFXUp = true;
                }
                //Debug.Log($"Cooldown restant avant cramé: {_remainingTime:F2} secondes");
            }
            else if(!_timerFinished)
            {
                _canAction = false;
                _timerIsActive = false;
                _timerFinished = true;
                _cauldronRecipeChecker.CheckQTE(false);
    
                if(!_isBurnVFXUp)
                {
                    _burnedVFX.Play();
                    _isBurnVFXUp = true;
                }
                //Debug.Log("Le cooldown est termin�. Vous pouvez ajouter un nouvel �l�ment !");
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
            _isBubbleVFXUp = false;
            _isFireVFXUp = false;

            _timerIsActive = true;
            _canAction = false;
        }

        public void ResetCooldown()
        {
            _timerFinished = false;
            _remainingTime = _itemCooldown;
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
            
            foreach(ParticleSystem fireVFX in _fireVFXArray)
            {
                fireVFX.Stop();
            }
        }
    }
}