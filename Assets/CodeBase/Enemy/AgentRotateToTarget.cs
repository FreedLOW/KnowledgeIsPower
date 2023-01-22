using UnityEngine;

namespace CodeBase.Enemy
{
    public class AgentRotateToTarget : Follow
    {
        [SerializeField] private float RotateSpeed;
        
        private Vector3 positionToLook;

        private void Update()
        {
            if (IsHeroTransformExist())
                RotateTowardHero();
        }

        private void RotateTowardHero()
        {
            UpdatePositionToLookAt();
            transform.rotation = SmoothedRotation(transform.rotation, positionToLook);
        }

        private void UpdatePositionToLookAt()
        {
            var positionDiff = heroTransform.position - transform.position;
            positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
            Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactory());

        private Quaternion TargetRotation(Vector3 position) => 
            Quaternion.LookRotation(position);

        private float SpeedFactory() => 
            RotateSpeed * Time.deltaTime;
    }
}