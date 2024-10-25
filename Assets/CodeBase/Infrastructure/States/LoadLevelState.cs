using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string SpawnerTag = "EnemySpawner";

        private readonly GameStateMachine gameStateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly LoadingCurtain loadingCurtain;
        private readonly IGameFactory gameFactory;
        private readonly IPersistentProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, 
            IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            this.gameStateMachine = gameStateMachine;
            this.sceneLoader = sceneLoader;
            this.loadingCurtain = loadingCurtain;
            this.gameFactory = gameFactory;
            _progressService = progressService;
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
            foreach (GameObject spawnerObject in GameObject.FindGameObjectsWithTag(SpawnerTag))
            {
                EnemySpawner enemySpawner = spawnerObject.GetComponent<EnemySpawner>();
                gameFactory.Register(enemySpawner);
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