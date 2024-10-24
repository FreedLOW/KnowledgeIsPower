using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        public EnemyAnimator Animator;

        [SerializeField] private float currentHP;
        [SerializeField] private float maxHP;

        public event Action OnHealthChanged;

        public float CurrentHP
        {
            get => currentHP;
            set => currentHP = value;
        }
        public float MaxHP
        {
            get => maxHP;
            set => maxHP = value;
        }

        public void TakeDamage(float damage)
        {
            CurrentHP -= damage;
            Animator.PlayHit();
            OnHealthChanged?.Invoke();
        }
    }
}