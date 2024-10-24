using System;

namespace CodeBase.Data
{
    [Serializable]
    public class HeroState
    {
        public float MaxHP;
        public float CurrentHP;

        public void ResetHP() => CurrentHP = MaxHP;
    }
}