using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        PlayerProgress LoadProgress();
        void SaveProgress();
    }
}