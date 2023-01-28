using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        public EnemyAnimator enemyAnimator;

        [SerializeField] private float maxHP;
        [SerializeField] private float currentHP;

        public event Action OnHealthChanged;

        public float MaxHP
        {
            get => maxHP;
            set => maxHP = value;
        }

        public float CurrentHP
        {
            get => currentHP;
            set => currentHP = value;
        }

        public void TakeDamage(float damage)
        {
            currentHP -= damage;
            enemyAnimator.HitAnimation();
            OnHealthChanged?.Invoke();
        }
    }
}