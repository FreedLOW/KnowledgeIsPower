using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar HpBar;

        private IHealth _health;

        private void Start()
        {
            var health = GetComponent<IHealth>();

            if (health != null)
                Construct(health);
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }

        public void Construct(IHealth health)
        {
            _health = health;
            _health.OnHealthChanged += OnHealthChanged;
        }
        
        private void OnHealthChanged() => 
            HpBar.SetValue(_health.CurrentHP, _health.MaxHP);
    }
}