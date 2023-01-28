using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Vertical = "Vertical";
        protected const string Horizontal = "Horizontal";
        private const string Button = "Fire1";
        
        public abstract Vector2 Axis { get; }

        public bool IsAttackButtonUp() => SimpleInput.GetButtonUp(Button);

        protected static Vector2 MobileAxis() => 
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}