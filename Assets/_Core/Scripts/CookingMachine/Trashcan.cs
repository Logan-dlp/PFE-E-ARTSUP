using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class Trashcan : MonoBehaviour
    {
        [SerializeField] private float _idleChangeTimer;
        
        private Animator _animator;
        private AnimatorControllerParameter[] _animatorControllerParameterArray;
        private float _timer;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _animatorControllerParameterArray = _animator.parameters;
        }
        public void DiscardItem()
        {
            _animator.SetTrigger(_animatorControllerParameterArray[1].name);
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
    }
}
