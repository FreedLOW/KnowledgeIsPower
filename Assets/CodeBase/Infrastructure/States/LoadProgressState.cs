using CodeBase.Data;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, 
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.PlayerProgress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        private void LoadProgressOrInitNew() => 
            _progressService.PlayerProgress = 
                _saveLoadService.LoadProgress() 
                ?? NewProgress();

        private static PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(initialLevel: "Main");
            progress.HeroState.MaxHP = 50f;
            progress.HeroState.ResetHP();
            progress.HeroStats.Damage = 1f;
            progress.HeroStats.RadiusDamage = 0.5f;
            return progress;
        }
    }
}