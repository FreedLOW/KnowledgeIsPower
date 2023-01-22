using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver triggerObserver;
        public Follow follower;
        public float cooldown;
        
        private Coroutine agroCoroutine;
        private bool hasAggroTarget;

        private void Start()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
            
            SwitchFollowerState(false);
        }

        private void TriggerEnter(Collider other)
        {
            if (!hasAggroTarget)
            {
                hasAggroTarget = true;
                StopAgroCoroutine();
                SwitchFollowerState(true);
            }
        }

        private void TriggerExit(Collider other)
        {
            if (hasAggroTarget)
            {
                hasAggroTarget = false;
                agroCoroutine = StartCoroutine(SwitchFollowerOffAfterCooldown());
            }
        }

        private void StopAgroCoroutine()
        {
            if (agroCoroutine != null)
            {
                StopCoroutine(agroCoroutine);
                agroCoroutine = null;
            }
        }

        private IEnumerator SwitchFollowerOffAfterCooldown()
        {
            yield return new WaitForSeconds(cooldown);
            SwitchFollowerState(false);
        }

        private void SwitchFollowerState(bool state) => 
            follower.enabled = state;
    }
}