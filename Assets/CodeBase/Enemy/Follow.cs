using UnityEngine;

namespace CodeBase.Enemy
{
    public abstract class Follow : MonoBehaviour
    {
        protected Transform heroTransform;

        public void Construct(Transform heroTransform)
        {
            this.heroTransform = heroTransform;
        }

        protected bool IsHeroTransformExist() => heroTransform != null;
    }
}