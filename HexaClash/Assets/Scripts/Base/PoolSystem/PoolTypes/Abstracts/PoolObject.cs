using UnityEngine;

namespace Base.PoolSystem.PoolTypes.Abstracts
{
    public abstract class PoolObject : MonoBehaviour
    {
        [SerializeField] PoolVariants poolVariant;

        public abstract void OnEnable();
        public abstract void ReturnPool();
    }
}