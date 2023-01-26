using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress
    {
        public HeroAnimator heroAnimator;

        private State heroState;

        public float MaxHeroHP
        {
            get => heroState.MaxHP;
            set => heroState.MaxHP = value;
        }
        public float CurrentHeroHP
        {
            get => heroState.CurrentHP;
            set
            {
                if (CurrentHeroHP != value)
                {
                    heroState.CurrentHP = value;
                    OnHealthChanged?.Invoke();
                }
            }
        }

        public Action OnHealthChanged;

        public void LoadProgress(PlayerProgress progress)
        {
            heroState = progress.HeroState;
            OnHealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.MaxHP = MaxHeroHP;
            progress.HeroState.CurrentHP = CurrentHeroHP;
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHeroHP <= 0) return;

            CurrentHeroHP -= damage;
            heroAnimator.PlayHit();
        }
    }
}