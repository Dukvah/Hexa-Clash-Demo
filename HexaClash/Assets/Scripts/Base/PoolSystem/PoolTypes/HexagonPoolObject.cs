using Base.PoolSystem.PoolTypes.Abstracts;
using Managers;
using UnityEngine;

namespace Base.PoolSystem.PoolTypes
{
    public class HexagonPoolObject : PoolObject
    {
        [SerializeField] MeshRenderer meshRenderer;
        public SOAllySoldierData SoAllySoldierData { get; set; }
        
        public override void OnEnable()
        {
            if (SoAllySoldierData != null)
            {
                meshRenderer.material = SoAllySoldierData.hexagonMaterial;
            }
        }

        private void OnDisable()
        {
            ReturnPool();
        }

        public override void ReturnPool()
        {
            if (!gameObject.activeSelf) return;
            
            EventManager.Instance?.returnHexagonPool.Invoke(this);
        }
    
    }
}