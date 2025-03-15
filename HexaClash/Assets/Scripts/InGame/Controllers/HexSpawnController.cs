using System.Collections.Generic;
using Managers;
using UnityEngine;

public class HexSpawnController : MonoBehaviour
{
    [SerializeField] private List<Transform> hexStackSpawnPoints = new();
    
    [SerializeField] private SOLevelHexStackData levelHexStacksDatas; //Hexagon stacks in the whole level
    [SerializeField] private HexStack hexStackPrefab;
    
    private List<HexStackData> currentWaveHexStackDatas = new();
    private List<HexStack> currentWaveHexStacks = new();
    
    private int  createdStackCount, notPlacedStackCount;

    public HexStack DraggedHexStack(GameObject draggedObject)
    {
        foreach (var hexStack in currentWaveHexStacks)
        {
            if (hexStack != null && draggedObject == hexStack.gameObject && hexStack.CanDrag)
                return hexStack;
        }
        return null;
    }
    
    public void RemoveStackFromList(HexStack stackToRemove)
    {
        if (stackToRemove != null && currentWaveHexStacks.Contains(stackToRemove))
        {
            currentWaveHexStacks.Remove(stackToRemove);
        }
    }
    
    public void SetNewStage(int phaseNo)
    {
        EventManager.Instance.onBattlePreparationsStart.Invoke(levelHexStacksDatas.waveHexStacks[phaseNo].hexStackDatas.Count);
        currentWaveHexStackDatas.Clear();
        currentWaveHexStacks.Clear();
        createdStackCount = 0;
        
        foreach (var waveHexStack in levelHexStacksDatas.waveHexStacks[phaseNo].hexStackDatas)
        {
            currentWaveHexStackDatas.Add(waveHexStack);
        }
        
        FillStackSpawnPoints();
    }

    private void FillStackSpawnPoints()
    {
        int remainingSpawnPoints = 3;
        
        for (int i = createdStackCount; i < currentWaveHexStackDatas.Count; i++)
        {
            remainingSpawnPoints--;
            
            var hexStack = Instantiate(hexStackPrefab, hexStackSpawnPoints[remainingSpawnPoints]);
            currentWaveHexStacks.Add(hexStack);
            hexStack.HexSpawnController = this;
            hexStack.HexStackData = currentWaveHexStackDatas[i];
            hexStack.gameObject.SetActive(true);
            
            createdStackCount++;
            notPlacedStackCount++;
            
            if (remainingSpawnPoints == 0 ) break;
        }
    }

    public void StackPlaced()
    {
        notPlacedStackCount--;
        EventManager.Instance.onStackPlaced.Invoke();
        if (notPlacedStackCount == 0)
        {
            FillStackSpawnPoints();
        }
    }
}


