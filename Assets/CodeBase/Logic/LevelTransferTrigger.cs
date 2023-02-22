using CodeBase.Infrastructure.State;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        public string levelTransferName;

        private IGameStateMachine gameStateMachine;
        private bool isTriggered;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }
        
        private void Start()
        {
            var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            
            if (activeSceneIndex + 1 <= sceneCount)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(activeSceneIndex + 1);
                levelTransferName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            }
            else if (activeSceneIndex - 1 != 0)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(activeSceneIndex - 1);
                levelTransferName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isTriggered) return;
            
            if (other.CompareTag(PlayerTag))
            {
                gameStateMachine.Enter<LoadLevelState, string>(levelTransferName);
                isTriggered = true;
            }
        }
    }
}