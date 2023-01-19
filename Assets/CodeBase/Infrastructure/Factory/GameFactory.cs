using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
        }

        public GameObject CreateHero(GameObject at) => 
            assetProvider.Instantiate(AssetsPath.HeroPath, at: at.transform.position);

        public void CreateHud() => 
            assetProvider.Instantiate(AssetsPath.HudPath);
    }
}