using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        event Action OnHealthChanged;
        float CurrentHP { get; set; }
        float MaxHP { get; set; }

        void TakeDamage(float damage);
    }
}