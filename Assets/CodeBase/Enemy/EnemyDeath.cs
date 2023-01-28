using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        public EnemyAnimator enemyAnimator;
        public EnemyHealth enemyHealth;
        public Follow follow;

        public GameObject deathVFX;

        public event Action OnEnemyDeath;
        
        private void Start()
        {
            enemyHealth.OnHealthChanged += OnOnHealthChanged;
        }

        private void OnDestroy()
        {
            enemyHealth.OnHealthChanged -= OnOnHealthChanged;
        }

        private void OnOnHealthChanged()
        {
            if (enemyHealth.CurrentHP <= 0)
                Die();
        }

        private void Die()
        {
            enemyHealth.OnHealthChanged -= OnOnHealthChanged;

            follow.enabled = false;
            enemyAnimator.DeathAnimation();
            SpawnDeathVFX();
            StartCoroutine(DestroyEnemyRoutine());
            
            OnEnemyDeath?.Invoke();
        }

        private void SpawnDeathVFX() => 
            Instantiate(deathVFX, transform.position, Quaternion.identity);

        private IEnumerator DestroyEnemyRoutine()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}