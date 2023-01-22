using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        public NavMeshAgent agent;

        private const float MinimalDistance = 1f;

        private void Update()
        {
            if (IsHeroTransformExist() && HeroNotReached())
                agent.destination = heroTransform.position;
        }

        private bool HeroNotReached() => 
            Vector3.Distance(agent.transform.position, heroTransform.position) >= MinimalDistance;
    }
}