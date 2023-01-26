using System;

namespace CodeBase.Data
{
    [Serializable]
    public class State
    {
        public float MaxHP;
        public float CurrentHP;

        public void ResetHP() => CurrentHP = MaxHP;
    }
}