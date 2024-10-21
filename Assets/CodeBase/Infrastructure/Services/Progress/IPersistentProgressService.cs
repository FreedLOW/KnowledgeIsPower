using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.Progress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress PlayerProgress { get; set; }
    }
}