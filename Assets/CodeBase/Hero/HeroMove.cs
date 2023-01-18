using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour
    {
        public CharacterController CharacterController;
        public float movementSpeed;

        private new Camera camera;
        private IInputService inputService;

        private void Awake()
        {
            inputService = Game.InputService;
            camera = Camera.main;
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = camera.transform.TransformDirection(inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
            
            CharacterController.Move(movementVector * movementSpeed * Time.deltaTime);
        }
    }
}
