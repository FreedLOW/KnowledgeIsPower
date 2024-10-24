using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth), typeof(HeroMove), typeof(HeroAnimator))]
    public class HeroDeath : MonoBehaviour
    {
        public HeroHealth HeroHealth;
        public HeroMove HeroMove;
        public HeroAttack HeroAttack;
        public HeroAnimator Animator;

        public GameObject DeathFx;
        
        private bool _isDead;

        private void Start()
        {
            HeroHealth.OnHealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            HeroHealth.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (!_isDead && HeroHealth.CurrentHP <= 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;

            HeroMove.enabled = false;
            HeroAttack.enabled = false;
            Animator.PlayDeath();

            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }
    }
}