using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar hpBar;

        private HeroHealth heroHealth;

        private void OnDestroy()
        {
            heroHealth.OnHealthChanged -= UpdateHPBar;
        }

        public void Construct(HeroHealth heroHealth)
        {
            this.heroHealth = heroHealth;
            this.heroHealth.OnHealthChanged += UpdateHPBar;
        }
        
        private void UpdateHPBar()
        {
            hpBar.SetValue(heroHealth.CurrentHeroHP, heroHealth.MaxHeroHP);
        }
    }
}