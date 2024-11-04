using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.Progress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Window;
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

            RegisterAdsService();

            IRandomService random = new RandomService();
            _allServices.RegisterSingle(random);

            IAssetProvider assetProvider = new AssetProvider();
            _allServices.RegisterSingle(assetProvider);
            
            IPersistentProgressService progressService = new PersistentProgressService();
            _allServices.RegisterSingle(progressService);

            _allServices.RegisterSingle<IUIFactory>(new UIFactory(assetProvider, _allServices.Single<IStaticDataService>(),
                progressService, _allServices.Single<IAdsService>()));
            _allServices.RegisterSingle<IWindowService>(new WindowService(_allServices.Single<IUIFactory>()));

            _allServices.RegisterSingle<IGameFactory>(new GameFactory(assetProvider, 
                _allServices.Single<IStaticDataService>(), random, progressService,
                _allServices.Single<IWindowService>()));
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
            staticData.LoadWindowConfigs();
            _allServices.RegisterSingle(staticData);
        }

        private void RegisterAdsService()
        {
            IAdsService adsService = new AdsService();
            adsService.Initialize();
            _allServices.RegisterSingle(adsService);
        }
    }
}