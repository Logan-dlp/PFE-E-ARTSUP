using MoonlitMixes.AI;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace MoonlitMixes.Health
{
    public class EnemyHealth : AHealth
    {
        [SerializeField] Material _hitMaterial;
        [SerializeField] float _timeHitVFX;
        [SerializeField] GameObject _dieVFX;
        [SerializeField] float _timeDieVFX;
        [SerializeField] private GameObject _itemDrop;

        private bool _hit = false;
        private float _time = 1;
        public override void TakeDamage(float damage)
        {
            RemoveHealth(damage);
            _hit = true;
            _time = 1;
            if (damage > 0) StartCoroutine(ActivateHitVFX());
        }
        [Button]
        public void  Test ()
        {
            _hit = true;
            _time = 1;
            StartCoroutine(ActivateHitVFX());
        }
        IEnumerator ActivateHitVFX ()
        {
            yield return new WaitForSeconds(_timeHitVFX / 100);

            if (_hit)
            {
                if (_time <= 0) { _hit = false; StartCoroutine(ActivateHitVFX());}
                else if (_time > 0)
                {
                    _time -= 0.05f;
                    _hitMaterial.SetFloat("_Split", _time);
                    StartCoroutine(ActivateHitVFX());
                }
            }
            else if (!_hit)
            {
                if (_time <= 1)
                {
                    _time += 0.05f;
                    _hitMaterial.SetFloat("_Split", _time);
                    StartCoroutine(ActivateHitVFX());
                }
                else _hit = true;
            }
        }
        protected override void CheckHealth()
        {
            if(_currentHealth <= 0)
            {
                Debug.Log("EnemyDead");
                GetComponent<Monster>().DropItem(_itemDrop);

                if (_dieVFX!=null) _dieVFX.SetActive(true);
            }
            
            healthBarScriptableInt.SendHealthAmount(_currentHealth / _maxHealth);
        }
    }
}