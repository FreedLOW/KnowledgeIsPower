using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        float MaxHP { get; set; }
        float CurrentHP { get; set; }
        event Action OnHealthChanged;
        void TakeDamage(float damage);
    }
}