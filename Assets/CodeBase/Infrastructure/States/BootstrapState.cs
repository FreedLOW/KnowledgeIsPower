using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
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
            _sceneLoader.Load(SceneName, onSceneLoaded: EnterLoadLevel);
        }

        public void Exit() { }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadLevelState, string>("Main");
        }

        private void RegisterServices()
        {
            _allServices.RegisterSingle<IInputService>(InputService());

            _allServices.RegisterSingle<IAssetProvider>(new AssetProvider());
            
            var gameFactory = new GameFactory(_allServices.Single<IAssetProvider>());
            _allServices.RegisterSingle<IGameFactory>(gameFactory);
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
            
            return new MobileInputService();
        }
    }
}