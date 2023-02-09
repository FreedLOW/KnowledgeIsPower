using CodeBase.CameraLogic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.State
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string EnemySpawnerTag = "EnemySpawner";

        private readonly GameStateMachine gameStateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly LoadingCurtain loadingCurtain;
        private readonly IGameFactory gameFactory;
        private readonly IPersistentProgressService progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
            IPersistentProgressService progressService)
        {
            this.gameStateMachine = gameStateMachine;
            this.sceneLoader = sceneLoader;
            this.loadingCurtain = loadingCurtain;
            this.gameFactory = gameFactory;
            this.progressService = progressService;
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

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(progressService.PlayerProgress);
            }
        }

        private void InitGameWorld()
        {
            InitializeSpawners();
            InitializeLootsSpawner();
            
            var hero = gameFactory.CreateHero(at: GameObject.FindWithTag(InitialPointTag));
            CameraFollow(hero);
            InitializeHUD(hero);
        }

        private void InitializeLootsSpawner()
        {
            if (progressService.PlayerProgress.LeftLoot.IdLeftLoots.Count <= 0) return;

            for (var i = 0; i < progressService.PlayerProgress.LeftLoot.IdLeftLoots.Count; i++)
            {
                LootPiece lootPiece = gameFactory.CreateLoot();
                Vector3Data lootPosition = progressService.PlayerProgress.LeftLoot.Loots[i].Position;
                lootPiece.transform.position = lootPosition.AsUnityVector();
                lootPiece.SetId(progressService.PlayerProgress.LeftLoot.IdLeftLoots[i]);
                lootPiece.Initialize(progressService.PlayerProgress.LeftLoot.Loots[i]);
                gameFactory.RegisterProgressReader(lootPiece);
            }
        }

        private void InitializeSpawners()
        {
            foreach (GameObject spawnObject in GameObject.FindGameObjectsWithTag(EnemySpawnerTag))
            {
                var spawner = spawnObject.GetComponent<EnemySpawner>();
                gameFactory.RegisterProgressReader(spawner);
            }
        }

        private void InitializeHUD(GameObject hero)
        {
            GameObject hud = gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }

        private void CameraFollow(GameObject follower) => 
            Camera.main.GetComponent<CameraFollow>().Follow(follower);
    }
}