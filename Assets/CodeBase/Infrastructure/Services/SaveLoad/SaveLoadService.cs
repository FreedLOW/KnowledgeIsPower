using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Progress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }
        
        public void SaveProgress()
        {
            foreach (ISavedProgress progressesWriter in _gameFactory.ProgressesWriters)
            {
                progressesWriter.UpdateProgress(_progressService.PlayerProgress);
            }
            
            PlayerPrefs.SetString(ProgressKey, _progressService.PlayerProgress.ToJson());
        }

        public PlayerProgress LoadProgress() => 
            PlayerPrefs.GetString(ProgressKey)
                ?.ToDeserialized<PlayerProgress>();
    }
}