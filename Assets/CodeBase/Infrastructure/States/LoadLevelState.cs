using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Loots;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly LoadingCurtain loadingCurtain;
        private readonly IGameFactory gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, 
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory)
        {
            this.gameStateMachine = gameStateMachine;
            this.sceneLoader = sceneLoader;
            this.loadingCurtain = loadingCurtain;
            this.gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            loadingCurtain.Show();
            gameFactory.Cleanup();
            gameFactory.WarmUp();
            sceneLoader.Load(sceneName, OnLoaded);
        }
        
        public void Exit() => 
            loadingCurtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();
            InformProgressReaders();

            gameStateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot() => 
            await _uiFactory.CreateUIRoot();

        private async Task InitGameWorld()
        {
            var levelData = LevelStaticData();

            InitSpawners(levelData);
            await InitLeftLoot();

            var hero = await InitHero(levelData);
            CameraFollow(hero);
            await InitHUD(hero);

            await InitLevelTransfer(levelData);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.PlayerProgress);
            }
        }

        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private void InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private async Task InitLeftLoot()
        {
            var leftLoots = _progressService.PlayerProgress.WorldData.LootData.LeftLoots;
            if (leftLoots.Count == 0)
                return;

            foreach (LeftLoot leftLoot in leftLoots)
            {
                LootPiece lootPiece = await gameFactory.SpawnLoot();
                lootPiece.Initialize(leftLoot.Loot);
                lootPiece.SetWorldPosition(leftLoot.Position);
                lootPiece.SetId(leftLoot.Id);
            }
        }

        private async Task<GameObject> InitHero(LevelStaticData levelData)
        {
            var hero = await gameFactory.CreateHero(at: levelData.InitialPoint);
            return hero;
        }

        private async Task InitHUD(GameObject hero)
        {
            var hud = await gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<IHealth>());
        }

        private void CameraFollow(GameObject follower) => 
            Camera.main.GetComponent<CameraFollow>().Follow(follower);

        private async Task InitLevelTransfer(LevelStaticData levelData)
        {
            var levelTransfer = await gameFactory.CreateLevelTransfer(levelData.LevelTransferPoint);
            levelTransfer.GetComponent<LevelTransfer>()
                .Construct(gameStateMachine, GetNextSceneName());
        }

        private static string GetNextSceneName()
        {
            Scene currentScene = SceneManager.GetActiveScene();
        
            int nextSceneIndex = (currentScene.buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        
            string nextScenePath = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
        
            string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextScenePath);

            return string.IsNullOrWhiteSpace(nextSceneName) ? string.Empty : nextSceneName;
        }
    }
}