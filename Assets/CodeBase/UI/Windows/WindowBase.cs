using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button closeButton;
        
        protected IPersistentProgressService progressService;
        
        protected PlayerProgress PlayerProgress => progressService.PlayerProgress;

        public void Construct(IPersistentProgressService progressService)
        {
            this.progressService = progressService;
        }
        
        private void Awake() => 
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() => 
            Cleanup();

        protected virtual void OnAwake() => 
            closeButton.onClick.AddListener(() => Destroy(gameObject));
        
        protected virtual void Initialize(){}
        protected virtual void SubscribeUpdates(){}
        protected virtual void Cleanup(){}
    }
}