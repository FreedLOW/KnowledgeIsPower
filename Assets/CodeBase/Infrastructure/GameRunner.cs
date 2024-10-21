using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper GameBootstrapper;

        private void Awake()
        {
            if (FindObjectOfType<GameBootstrapper>() == null) 
                Instantiate(GameBootstrapper);
        }
    }
}