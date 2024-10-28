using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.Loots;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";

        private readonly GameStateMachine gameStateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly LoadingCurtain loadingCurtain;
        private readonly IGameFactory gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, 
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData)
        {
            this.gameStateMachine = gameStateMachine;
            this.sceneLoader = sceneLoader;
            this.loadingCurtain = loadingCurtain;
            this.gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
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
            InitGameWorld();
            InformProgressReaders();

            gameStateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            InitSpawners();
            InitLeftLoot();
            
            var hero = gameFactory.CreateHero(at: GameObject.FindWithTag(InitialPointTag));
            CameraFollow(hero);
            InitHUD(hero);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.PlayerProgress);
            }
        }

        private void InitSpawners()
        {
            string levelKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(levelKey);

            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitLeftLoot()
        {
            var leftLoots = _progressService.PlayerProgress.WorldData.LootData.LeftLoots;
            if (leftLoots.Count == 0)
                return;

            foreach (LeftLoot leftLoot in leftLoots)
            {
                LootPiece lootPiece = gameFactory.SpawnLoot();
                lootPiece.Initialize(leftLoot.Loot);
                lootPiece.SetWorldPosition(leftLoot.Position);
                lootPiece.SetId(leftLoot.Id);
            }
        }

        private void InitHUD(GameObject hero)
        {
            var hud = gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<IHealth>());
        }

        private void CameraFollow(GameObject follower) => 
            Camera.main.GetComponent<CameraFollow>().Follow(follower);
    }
}