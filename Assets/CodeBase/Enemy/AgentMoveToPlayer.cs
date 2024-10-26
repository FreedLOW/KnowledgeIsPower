using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        public NavMeshAgent Agent;

        private const float MinDistance = 1f;

        private void Update()
        {
            if (HasTarget() && TargetNotReached())
            {
                Agent.destination = Target.position;
            }
        }

        private bool HasTarget() =>
            Target != null;

        private bool TargetNotReached() =>
            Vector3.Distance(Agent.transform.position, Target.position) >= MinDistance;
    }
}