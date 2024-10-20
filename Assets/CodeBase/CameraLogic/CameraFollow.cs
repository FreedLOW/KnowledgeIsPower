using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform following;
        [SerializeField] private float RotationAngleX;
        [SerializeField] private float Distance;
        [SerializeField] private float OffsetY;

        private void LateUpdate()
        {
            if (following == null) return;

            var rotation = Quaternion.Euler(RotationAngleX, 0, 0);
            var position = rotation * new Vector3(0, 0, -Distance) + FollowingPointPosition();
            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject follower) => 
            following = follower.transform;

        private Vector3 FollowingPointPosition()
        {
            Vector3 followingPosition = following.position;
            followingPosition.y += OffsetY;
            return followingPosition;
        }
    }
}