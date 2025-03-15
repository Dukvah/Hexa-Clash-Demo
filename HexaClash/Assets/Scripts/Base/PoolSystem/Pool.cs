using System.Collections.Generic;
using Base.PoolSystem.PoolTypes.Abstracts;
using Managers;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [Header("Pool Type")]
    [SerializeField] PoolVariants poolVariant;

    [Header("Pool Prefab")]
    [SerializeField] PoolObject poolGameObject;
    
    [Header("Pool Size")]
    [SerializeField] int poolStartSize = 10;
    
    
    private Queue<PoolObject> poolObjects = new();
    
    private void Awake() 
    {
        FillPool();
    }

    private void OnEnable()
    {
        switch ((int)poolVariant)
        {
            case 0:
                EventManager.Instance.returnHexagonPool.AddListener(ReturnPool);
                break;
        }
        
    }
    
    public PoolObject GetPooledObject()
    {
        while (poolObjects.Count < 1)
        {
            CreatePoolObject();
        }
        
        var obj = poolObjects.Dequeue();
        return obj;
    }

    private void CreatePoolObject()
    {
        PoolObject poolObject = Instantiate(poolGameObject, gameObject.transform);
        poolObject.gameObject.SetActive(false);
        poolObjects.Enqueue(poolObject);
    }
    private void FillPool()
    {
        for (int i = 0; i < poolStartSize; i++)
        {
            CreatePoolObject();
        }
    }
    private void ReturnPool(PoolObject obj)
    {
        obj.gameObject.SetActive(false);
        poolObjects.Enqueue(obj);
    }
    
}

public enum PoolVariants { HexagonPool = 0, }