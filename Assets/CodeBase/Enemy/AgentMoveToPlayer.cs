using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        public NavMeshAgent Agent;

        private const float MinDistance = 1f;

        private Transform _target;

        private void Update()
        {
            if (HasTarget() && TargetNotReached())
            {
                Agent.destination = _target.position;
            }
        }

        private bool HasTarget() =>
            _target != null;

        private bool TargetNotReached() =>
            Vector3.Distance(Agent.transform.position, _target.position) >= MinDistance;
    }
}