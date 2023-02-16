using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar hpBar;

        private IHealth health;

        public void Construct(IHealth health)
        {
            this.health = health;
            this.health.OnHealthChanged += UpdateHPBar;
        }

        private void Start()
        {
            IHealth health = GetComponent<IHealth>();

            if (health != null)
                Construct(health);
        }

        private void OnDestroy()
        {
            health.OnHealthChanged -= UpdateHPBar;
        }

        private void UpdateHPBar()
        {
            hpBar.SetValue(health.CurrentHP, health.MaxHP);
        }
    }
}