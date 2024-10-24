using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        public HeroAnimator Animator;
        
        private HeroState _heroState;

        public event Action OnHealthChanged;

        public float CurrentHP
        {
            get => _heroState.CurrentHP;
            set
            {
                if (CurrentHP != value)
                {
                    _heroState.CurrentHP = value;
                    OnHealthChanged?.Invoke();
                }
            }
        }
        public float MaxHP
        {
            get => _heroState.MaxHP;
            set => _heroState.MaxHP = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _heroState = progress.HeroState;
            OnHealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.MaxHP = MaxHP;
            progress.HeroState.CurrentHP = CurrentHP;
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHP <= 0)
                return;

            CurrentHP -= damage;
            Animator.PlayHit();
        }
    }
}