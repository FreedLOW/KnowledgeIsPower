using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        public float RotationSpeed = 5f;
        
        private Transform _target;
        private Vector3 _positionToLook;

        private void Update()
        {
            if (HasTarget()) 
                RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private bool HasTarget() =>
            _target != null;

        private void UpdatePositionToLookAt()
        {
            var direction = (transform.position - _target.position).normalized;
            _positionToLook = new Vector3(direction.x, transform.position.y, direction.z);
        }

        private Quaternion SmoothedRotation(Quaternion transformRotation, Vector3 positionToLook) => 
            Quaternion.Lerp(transformRotation, TargetRotation(positionToLook), SmoothSpeed());

        private static Quaternion TargetRotation(Vector3 positionToLook) => 
            Quaternion.LookRotation(positionToLook);

        private float SmoothSpeed() => 
            RotationSpeed * Time.deltaTime;
    }
}