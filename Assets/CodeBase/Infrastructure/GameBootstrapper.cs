using CodeBase.Infrastructure.State;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain LoadingCurtainPrefab;
        private Game game;

        private void Awake()
        {
            game = new Game(this, Instantiate(LoadingCurtainPrefab));
            game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}