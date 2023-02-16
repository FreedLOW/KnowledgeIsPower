using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        public Button button;
        public WindowId windowId;
        
        private IWindowService windowService;

        public void Construct(IWindowService windowService) => 
            this.windowService = windowService;

        private void Awake() => 
            button.onClick.AddListener(Open);

        private void Open() => 
            windowService.Open(windowId);
    }
}