using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Window
{
    public interface IWindowService : IService
    {
        void Open(WindowId windowId);
    }
}