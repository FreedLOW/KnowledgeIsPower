using UnityEngine;

namespace CodeBase.Enemy
{
    public abstract class Follow : MonoBehaviour
    {
        protected Transform Target;

        public void Construct(Transform hero)
        {
            Target = hero;
        }

        public void RemoveTarget() => 
            Target = null;
    }
}