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
            gameFactory.Cleanup();
            sceneLoader.Load(sceneName, OnLoaded);
        }
        
        public void Exit() => loadingCurtain.Hide();

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
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

        private void InitUIRoot() => 
            uiFactory.CreateRoot();

        private void InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            InitializeSpawners(levelData);
            InitializeLeftLootsSpawner();
            
            GameObject hero = InitializeHero(levelData);
            CameraFollow(hero);
            InitializeHUD(hero);

            InitializeTransferTrigger(levelData);
        }

        private LevelStaticData LevelStaticData() => 
            staticDataService.ForLevel(SceneManager.GetActiveScene().name);

        private void InitializeSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitializeLeftLootsSpawner()
        {
            if (progressService.PlayerProgress.LeftLoot.IdLeftLoots.Count <= 0) return;

            for (var i = 0; i < progressService.PlayerProgress.LeftLoot.IdLeftLoots.Count; i++)
            {
                Loot leftLoot = progressService.PlayerProgress.LeftLoot.Loots[i];
                var leftLootId = progressService.PlayerProgress.LeftLoot.IdLeftLoots[i];
                gameFactory.CreateLeftLoot(leftLoot, leftLootId);
            }
        }

        private GameObject InitializeHero(LevelStaticData levelData) => 
            gameFactory.CreateHero(levelData.InitialHeroPosition);

        private void InitializeHUD(GameObject hero)
        {
            GameObject hud = gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }

        private void CameraFollow(GameObject follower) => 
            Camera.main.GetComponent<CameraFollow>().Follow(follower);

        private void InitializeTransferTrigger(LevelStaticData levelData)
        {
            LevelTransferTrigger transferTrigger = gameFactory.CreateLevelTransferTrigger(levelData.TransferTriggerPosition);
            transferTrigger.Construct(gameStateMachine);
        }
    }
}