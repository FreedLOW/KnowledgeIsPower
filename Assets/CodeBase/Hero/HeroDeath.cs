using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        public HeroHealth heroHealth;
        public HeroMove heroMove;
        public HeroAnimator heroAnimator;

        public GameObject deathVFX;
        
        private bool isDead;

        private void Start()
        {
            heroHealth.OnHealthChanged+= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            if (!isDead && heroHealth.CurrentHeroHP <= 0) 
                Die();
        }

        private void Die()
        {
            isDead = true;
            heroMove.enabled = false;
            heroAnimator.PlayDeath();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }
    }
}