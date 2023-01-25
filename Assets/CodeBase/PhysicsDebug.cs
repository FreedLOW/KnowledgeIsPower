using UnityEngine;

namespace CodeBase
{
    public static class PhysicsDebug
    {
        public static void DrawDebug(Vector3 worldPosition, float radius, float visibleTime)
        {
            Debug.DrawRay(worldPosition, radius * Vector3.up, Color.red, visibleTime);
            Debug.DrawRay(worldPosition, radius * Vector3.down, Color.red, visibleTime);
            Debug.DrawRay(worldPosition, radius * Vector3.left, Color.red, visibleTime);
            Debug.DrawRay(worldPosition, radius * Vector3.right, Color.red, visibleTime);
            Debug.DrawRay(worldPosition, radius * Vector3.forward, Color.red, visibleTime);
            Debug.DrawRay(worldPosition, radius * Vector3.back, Color.red, visibleTime);
        }
    }
}