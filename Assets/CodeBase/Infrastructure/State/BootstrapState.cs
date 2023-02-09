using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.State
{
    public class BootstrapState : IState
    {
        private const string SceneName = "Initial";
        
        private readonly GameStateMachine gameStateMachine;
        private readonly SceneLoader sceneLoader;
        private readonly AllServices allServices;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices services)
        {
            this.gameStateMachine = gameStateMachine;
            this.sceneLoader = sceneLoader;
            allServices = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            sceneLoader.Load(SceneName, onSceneLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            gameStateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            allServices.RegisterSingle(InputService());
            allServices.RegisterSingle<IAssetProvider>(new AssetProvider());
            IRandomService randomService = new UnityRandomService();
            allServices.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            allServices.RegisterSingle<IGameFactory>(new GameFactory(allServices.Single<IAssetProvider>(), 
                allServices.Single<IStaticDataService>(), randomService, allServices.Single<IPersistentProgressService>()));
            allServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(allServices.Single<IGameFactory>(),
                allServices.Single<IPersistentProgressService>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMonsters();
            allServices.RegisterSingle(staticData);
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            else
                return new MobileInputService();
        }
    }
}