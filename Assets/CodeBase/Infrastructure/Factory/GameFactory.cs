using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider assetProvider;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressesWriters { get; } = new List<ISavedProgress>();
        public GameObject HeroGameObject { get; private set; }
        
        public event Action HeroCreated;

        public GameFactory(IAssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
        }

        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegister(prefabPath: AssetsPath.HeroPath, at: at.transform.position);
            HeroCreated?.Invoke();
            return HeroGameObject;
        }

        public GameObject CreateHud() => 
            InstantiateRegister(AssetsPath.HudPath);

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressesWriters.Clear();
        }

        private GameObject InstantiateRegister(string prefabPath, Vector3 at)
        {
            var gameObject = assetProvider.Instantiate(prefabPath, at: at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        private GameObject InstantiateRegister(string prefabPath)
        {
            var gameObject = assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                RegisterProgressReader(progressReader);
            }
        }

        private void RegisterProgressReader(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressesWriters.Add(progressWriter);
            
            ProgressReaders.Add(progressReader);
        }
    }
}