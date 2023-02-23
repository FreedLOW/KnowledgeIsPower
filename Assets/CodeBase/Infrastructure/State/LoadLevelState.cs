using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.State
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly LoadingCurtain loadingCurtain;
        private readonly IGameFactory gameFactory;
        private readonly IPersistentProgressService progressService;
        private readonly IStaticDataService staticDataService;
        private readonly IUIFactory uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
            IPersistentProgressService progressService, IStaticDataService staticDataService, IUIFactory uiFactory)
        {
            this.gameStateMachine = gameStateMachine;
            this.sceneLoader = sceneLoader;
            this.loadingCurtain = loadingCurtain;
            this.gameFactory = gameFactory;
            this.progressService = progressService;
            this.staticDataService = staticDataService;
            this.uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            loadingCurtain.Show();
            gameFactory.CleanUp();
            gameFactory.WarmUp();
            sceneLoader.Load(sceneName, OnLoaded);
        }
        
        public void Exit() => loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();

            gameStateMachine.Enter<GameLoopState>();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(progressService.PlayerProgress);
            }
        }

        private async Task InitUIRoot() => 
            await uiFactory.CreateRoot();

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            await InitializeSpawners(levelData);
            await InitializeLeftLootsSpawner();
            
            GameObject hero = await InitializeHero(levelData);
            await InitializeHUD(hero);
            CameraFollow(hero);

            await InitializeTransferTrigger(levelData);
        }

        private LevelStaticData LevelStaticData() => 
            staticDataService.ForLevel(SceneManager.GetActiveScene().name);

        private async Task InitializeSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private async Task InitializeLeftLootsSpawner()
        {
            if (progressService.PlayerProgress.LeftLoot.IdLeftLoots.Count <= 0) return;

            for (var i = 0; i < progressService.PlayerProgress.LeftLoot.IdLeftLoots.Count; i++)
            {
                Loot leftLoot = progressService.PlayerProgress.LeftLoot.Loots[i];
                var leftLootId = progressService.PlayerProgress.LeftLoot.IdLeftLoots[i];
                await gameFactory.CreateLeftLoot(leftLoot, leftLootId);
            }
        }

        private async Task<GameObject> InitializeHero(LevelStaticData levelData) => 
            await gameFactory.CreateHero(levelData.InitialHeroPosition);

        private async Task InitializeHUD(GameObject hero)
        {
            GameObject hud = await gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }

        private void CameraFollow(GameObject follower) => 
            Camera.main.GetComponent<CameraFollow>().Follow(follower);

        private async Task InitializeTransferTrigger(LevelStaticData levelData)
        {
            LevelTransferTrigger transferTrigger = await gameFactory.CreateLevelTransferTrigger(levelData.TransferTriggerPosition);
            transferTrigger.Construct(gameStateMachine);
        }
    }
}