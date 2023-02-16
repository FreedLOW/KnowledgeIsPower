using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            this.uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.None:
                    break;
                case WindowId.Shop:
                    uiFactory.CreateShop();
                    break;
            }
        }
    }
}