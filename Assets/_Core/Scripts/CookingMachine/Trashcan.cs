using MoonlitMixes.Player;
using System.Collections;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class Trashcan : MonoBehaviour
    {
        [SerializeField] private float _idleChangeTimer;
        [SerializeField] private GameObject _uiPrompt;

        private AnimatorControllerParameter[] _animatorControllerParameterArray;
        private Animator _animator;
        private float _timer;

        private PlayerInteraction _playerInside;
        private bool _mouthOpen = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animatorControllerParameterArray = _animator.parameters;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            // Animation idle uniquement si la bouche est ferm�e
            if (_timer >= _idleChangeTimer && !_mouthOpen)
            {
                _animator.SetInteger(_animatorControllerParameterArray[2].name, Random.Range(0, 2));
                _animator.SetTrigger(_animatorControllerParameterArray[0].name);
                _timer = 0;
            }

            // V�rifie si le joueur a jet� l'objet sans sortir du trigger
            if (_playerInside != null && _mouthOpen && _playerInside.ItemInHand == null)
            {
                CloseMouth();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 10) return;

            PlayerInteraction player = other.GetComponent<PlayerInteraction>();
            if (player != null)
            {
                _playerInside = player;
                if (player.ItemInHand != null)
                {
                    Debug.Log("Test");
                    player.SetCurrentTrashcan(this);
                    OpenMouth();

                    if (_uiPrompt != null)
                    {
                        _uiPrompt.SetActive(true);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != 10) return;

            PlayerInteraction player = other.GetComponent<PlayerInteraction>();
            if (player != null && player.ItemInHand != null && !_mouthOpen)
            {
                OpenMouth();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 10) return;

            PlayerInteraction player = other.GetComponent<PlayerInteraction>();
            if (_playerInside!=null&& player == _playerInside)
            {
                _playerInside = null;
                player.ClearCurrentTrashcan(this);

                CloseMouth();
            }
        }

        private void OpenMouth()
        {
            Debug.Log(_animatorControllerParameterArray[3].name);
            _mouthOpen = true;
            _animator.SetBool(_animatorControllerParameterArray[3].name, true);
        }

        private void CloseMouth()
        {
            _mouthOpen = false;
            _animator.SetBool(_animatorControllerParameterArray[3].name, false);
        }

        public void DiscardItem()
        {
            _animator.SetTrigger(_animatorControllerParameterArray[1].name);
            StartCoroutine(WaitAnim());
        }

        private IEnumerator WaitAnim()
        {
            yield return new WaitForSeconds(0.1f);
            CloseMouth();

            if (_uiPrompt != null)
            {
                _uiPrompt.SetActive(false);
            }
        }
    }
}