using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public abstract class Follow : MonoBehaviour
    {
        protected Transform heroTransform;
        protected IGameFactory gameFactory;
        
        private void Start()
        {
            gameFactory = AllServices.Container.Single<IGameFactory>();

            if (IsHeroExist())
                InitializeHeroTransform();
            else
                gameFactory.HeroCreated += OnHeroCreated;
        }
        
        protected bool IsHeroTransformExist() => heroTransform != null;

        private void OnHeroCreated() => 
            InitializeHeroTransform();

        private void InitializeHeroTransform() => 
            heroTransform = gameFactory.HeroGameObject.transform;

        private bool IsHeroExist() => 
            gameFactory.HeroGameObject != null;
    }
}