using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransfer : MonoBehaviour
    {
        public string TransferTo;
        
        private bool _isTriggered;
        
        private IGameStateMachine _stateMachine;

        public void Construct(IGameStateMachine stateMachine, string nextSceneName)
        {
            _stateMachine = stateMachine;
            TransferTo = nextSceneName;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isTriggered)
                return;

            if (other.CompareTag("Player"))
            {
                _isTriggered = true;
                _stateMachine.Enter<LoadLevelState, string>(TransferTo);
            }
        }
    }
}