using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.Progress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress PlayerProgress { get; set; }
    }
}