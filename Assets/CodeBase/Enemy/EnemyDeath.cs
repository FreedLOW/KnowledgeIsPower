using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        public EnemyHealth EnemyHealth;
        public EnemyAnimator Animator;

        public GameObject DeathFx;

        public event Action OnDeath;

        private void Start()
        {
            EnemyHealth.OnHealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            EnemyHealth.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (EnemyHealth.CurrentHP <= 0f) 
                Die();
        }

        private void Die()
        {
            EnemyHealth.OnHealthChanged -= OnHealthChanged;
            
            Animator.PlayDeath();

            SpawnDeathFx();
            StartCoroutine(DestroyTimerRoutine());
            
            OnDeath?.Invoke();
        }

        private void SpawnDeathFx() => 
            Instantiate(DeathFx, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimerRoutine()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}