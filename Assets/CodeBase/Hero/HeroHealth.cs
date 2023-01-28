using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth, ISavedProgress
    {
        public HeroAnimator heroAnimator;

        private State heroState;

        public float MaxHP
        {
            get => heroState.MaxHP;
            set => heroState.MaxHP = value;
        }
        public float CurrentHP
        {
            get => heroState.CurrentHP;
            set
            {
                if (CurrentHP != value)
                {
                    heroState.CurrentHP = value;
                    OnHealthChanged?.Invoke();
                }
            }
        }

        public event Action OnHealthChanged;

        public void LoadProgress(PlayerProgress progress)
        {
            heroState = progress.HeroState;
            OnHealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.MaxHP = MaxHP;
            progress.HeroState.CurrentHP = CurrentHP;
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHP <= 0) return;

            CurrentHP -= damage;
            heroAnimator.PlayHit();
        }
    }
}