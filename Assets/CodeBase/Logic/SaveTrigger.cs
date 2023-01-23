using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        public BoxCollider collider;
        
        private ISaveLoadService saveLoadService;

        private void Awake()
        {
            saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void OnTriggerEnter(Collider other)
        {
            saveLoadService.SaveProgress();
            Debug.Log("save complete");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (collider == null) return;

            Gizmos.color = new Color32(40, 250, 50, 150);
            Gizmos.DrawCube(transform.position + collider.center, collider.size);
        }
    }
}