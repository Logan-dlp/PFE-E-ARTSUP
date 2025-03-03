using MoonlitMixes.CookingMachine;
using UnityEngine;

public class CauldronTimer : MonoBehaviour
{
    [SerializeField] private float _itemCooldown = 60f;
    
    private CauldronRecipeChecker _cauldronRecipeChecker;
    private bool _timerFinished;
    private bool _timerIsActive;
    private bool _canAction = true;
    private float _remainingTime;
    
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

    public void Start()
    {
        ResetCooldown();
    }

    private void FixedUpdate()
    {        
        if(!_timerIsActive) return;
        
        if (_remainingTime >= _itemCooldown/2)
        {
            _canAction = false;
            _remainingTime -= Time.fixedDeltaTime;
            //Debug.Log($"Cooldown restant: {_remainingTime:F2} secondes");
        }
        else if (_remainingTime >= 0)
        {   
            _canAction = true;
            _remainingTime -= Time.fixedDeltaTime;
            //Debug.Log($"Cooldown restant avant cramé: {_remainingTime:F2} secondes");
        }
        else if(!_timerFinished)
        {
            _canAction = false;
            _timerIsActive = false;
            _timerFinished = true;
            _cauldronRecipeChecker.NeedMix = false;
            _cauldronRecipeChecker.CheckQTE(false);
            //Debug.Log("Le cooldown est termin�. Vous pouvez ajouter un nouvel �l�ment !");
        }
    }

    public void ResetCooldown()
    {
        _remainingTime = _itemCooldown;
    }

    public void StopCooldown()
    {
        _remainingTime = _itemCooldown;
        _timerIsActive = false;
    }
}