using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string SceneName = "Initial";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _allServices = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(SceneName, onSceneLoaded: EnterLoadProgress);
        }

        public void Exit() { }

        private void EnterLoadProgress()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _allServices.RegisterSingle(InputService());

            RegisterStaticDataService();

            IRandomService random = new RandomService();
            _allServices.RegisterSingle(random);
            
            IPersistentProgressService progressService = new PersistentProgressService();
            _allServices.RegisterSingle(progressService);

            _allServices.RegisterSingle<IAssetProvider>(new AssetProvider());
            _allServices.RegisterSingle<IGameFactory>(new GameFactory(_allServices.Single<IAssetProvider>(),
                _allServices.Single<IStaticDataService>(), random, progressService));
            _allServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(progressService,
                _allServices.Single<IGameFactory>()));
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            
            return new MobileInputService();
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMonsters();
            staticData.LoadLevels();
            _allServices.RegisterSingle(staticData);
        }
    }
}