using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            Quaternion rotation = camera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.back, rotation * Vector3.up);
        }
    }
}