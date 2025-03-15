using System.Collections.Generic;
using Base.PoolSystem.PoolTypes;
using Base.PoolSystem.PoolTypes.Abstracts;
using DG.Tweening;
using Managers;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    public HexSpawnController HexSpawnController { get; set; }
    public HexStackData HexStackData { get; set; }
    public List<HexagonPoolObject> hexPoolObjects = new();
    
    [SerializeField] public SpriteRenderer stackIconSprite;
    

    public bool CanDrag { get; set; }
    private void OnEnable()
    {
        InitStack();
    }
    
    private void InitStack()
    {
        var spawnOffset = Vector3.zero;
        if (HexStackData == null) return;
        
        foreach (var hexStackObject in HexStackData.hexStackObjects)
        {
            PoolObject pooledObject  = GameManager.Instance.HexagonPool.GetPooledObject();
            if (pooledObject is HexagonPoolObject hexagon)
            {
                HexagonPoolObject newHexagon = hexagon;
                newHexagon.transform.parent = transform;
                newHexagon.transform.localRotation = Quaternion.identity;
                newHexagon.transform.position = transform.position + spawnOffset;
                hexPoolObjects.Add(newHexagon);
                newHexagon.SoAllySoldierData = hexStackObject.allySoldierData; 
                newHexagon.gameObject.SetActive(true);
                
                if (hexStackObject == HexStackData.hexStackObjects[^1])
                {
                    stackIconSprite.sprite = hexStackObject.allySoldierData.menuSprite;
                    spawnOffset.z -= 0.075f;
                    stackIconSprite.transform.position = transform.position + spawnOffset;
                }
                
                spawnOffset.z -= 0.15f;
            }
        }
        
        CanDrag = true;
    }
    
    public void StackPlaced()
    {
        HexSpawnController.StackPlaced();
    }
    
}
