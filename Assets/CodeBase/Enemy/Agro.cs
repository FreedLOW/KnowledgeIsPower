using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Agro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;

        public float Cooldown;

        private bool _hasAggroTarget;
        private Coroutine _agroCoroutine;

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;
            
            FollowSwitchOff();
        }

        private void TriggerEnter(Collider other)
        {
            if (!_hasAggroTarget)
            {
                _hasAggroTarget = true;
                StopAgroCoroutine();
                FollowSwitchOn();
            }
        }

        private void TriggerExit(Collider other)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;
                _agroCoroutine = StartCoroutine(SwitchFollowAfterCooldownRoutine());
            }
        }

        private void FollowSwitchOn() => 
            Follow.enabled = true;

        private void FollowSwitchOff() => 
            Follow.enabled = false;

        private void StopAgroCoroutine()
        {
            if (_agroCoroutine != null)
            {
                StopCoroutine(_agroCoroutine);
                _agroCoroutine = null;
            }
        }

        private IEnumerator SwitchFollowAfterCooldownRoutine()
        {
            yield return new WaitForSeconds(Cooldown);
            FollowSwitchOff();
        }
    }
}