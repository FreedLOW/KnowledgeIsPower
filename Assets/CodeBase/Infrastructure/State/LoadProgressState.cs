using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;

namespace CodeBase.Infrastructure.State
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly IPersistentProgressService persistentProgressService;
        private readonly ISaveLoadService saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService)
        {
            this.gameStateMachine = gameStateMachine;
            this.persistentProgressService = persistentProgressService;
            this.saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            gameStateMachine.Enter<LoadLevelState, string>(persistentProgressService.PlayerProgress.WorldData.PositionOnLevel.LevelName);
        }

        public void Exit()
        {
            
        }

        private void LoadProgressOrInitNew()
        {
            persistentProgressService.PlayerProgress = saveLoadService.LoadProgress() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(initialLevel: "Main");
            progress.HeroState.MaxHP = 50;
            progress.HeroState.ResetHP();
            return progress;
        }
    }
}