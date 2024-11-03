using CodeBase.UI.Services.Window;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        public Button Button;
        public WindowId WindowId;
        
        private IWindowService _windowService;

        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        private void Awake()
        {
            Button.onClick.AddListener(OnOpenWindow);
        }

        private void OnOpenWindow() => 
            _windowService.Open(WindowId);
    }
}