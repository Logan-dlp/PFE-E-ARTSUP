using System.Collections;
using MoonlitMixes.Events;
using MoonlitMixes.Player;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class Trashcan : MonoBehaviour
    {
        [SerializeField] private float _idleChangeTimer;
        [SerializeField] private ScriptableBoolEvent _scriptableBoolEvent;

        private AnimatorControllerParameter[] _animatorControllerParameterArray;
        private Animator _animator;
        private float _timer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animatorControllerParameterArray = _animator.parameters;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _idleChangeTimer)
            {
                _animator.SetInteger(_animatorControllerParameterArray[2].name, Random.Range(0, 2));
                _animator.SetTrigger(_animatorControllerParameterArray[0].name);
                _timer = 0;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(other.gameObject.layer == 10 && other.GetComponent<PlayerInteraction>().ItemInHand != null)
            {
                AnimMouth(true);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.gameObject.layer == 10)
            {
                AnimMouth(false);
            }
        }

        private void AnimMouth(bool state)
        {
            if(state)
            {
                _animator.SetBool(_animatorControllerParameterArray[3].name, true);
            }
            else
            {
                _animator.SetBool(_animatorControllerParameterArray[3].name, false); 
            }
        }

        public void DiscardItem()
        {
            _animator.SetTrigger(_animatorControllerParameterArray[1].name);
            StartCoroutine(WaitAnim());
        }

        private IEnumerator WaitAnim()
        {
            yield return new WaitForSeconds(.1f);
            AnimMouth(false);
        }
    }
}
