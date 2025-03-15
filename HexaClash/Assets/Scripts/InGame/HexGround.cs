using System;
using System.Collections;
using System.Collections.Generic;
using Base.PoolSystem.PoolTypes;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;


public class HexGround : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material originalMaterial, hoveredMaterial;
    [SerializeField] private Transform stackStartPos;

    public List<HexGround> neighborHexGrounds = new();
    public Vector3 HexStackPoint { get; private set; }
    public HexStack CurrentHexStack { get; set; }
    public bool IsEmpty { get; private set; }

    private void Start()
    {
        HexStackPoint = stackStartPos.position;
        IsEmpty = true;
    }

    public void OnNewStackEnter(HexStack newHexStack)
    {
        newHexStack.StackPlaced();
        IsEmpty = false;
        CurrentHexStack = newHexStack;
        CurrentHexStack.CanDrag = false;

        if (CurrentHexStack.hexPoolObjects.Count > 0)
        {
            int topIndex = CurrentHexStack.hexPoolObjects.Count - 1;
            StartTransferSequence(CurrentHexStack.hexPoolObjects[topIndex]);
        }
    }

    private void StartTransferSequence(HexagonPoolObject topHexagon)
    {
        List<HexGround> matchingNeighbors = new List<HexGround>();

        foreach (var neighbor in neighborHexGrounds)
        {
            if (!neighbor.CurrentHexStack.IsUnityNull() && !neighbor.IsEmpty)
            {
                HexagonPoolObject neighborTopHexagon = neighbor.GetTopHexagon();

                if (topHexagon.SoAllySoldierData == neighborTopHexagon.SoAllySoldierData)
                {
                    matchingNeighbors.Add(neighbor);
                }
            }
        }

        if (matchingNeighbors.Count > 0)
        {
            ProcessNextTransfer(matchingNeighbors, 0);
        }
    }

    private void ProcessNextTransfer(List<HexGround> matchingNeighbors, int index)
    {
        if (index < matchingNeighbors.Count)
        {
            HexGround neighbor = matchingNeighbors[index];
            if (!neighbor.IsEmpty && !neighbor.CurrentHexStack.IsUnityNull())
            {
                HexagonPoolObject neighborTopHexagon = neighbor.GetTopHexagon();
                HexagonPoolObject ourTopHexagon = GetTopHexagon();

                if (neighborTopHexagon.SoAllySoldierData == ourTopHexagon.SoAllySoldierData)
                {
                    TransferSingleHexagonWithCallback(neighbor, neighborTopHexagon, () => {});
                    
                    DOVirtual.DelayedCall(0.13f, () => {
                        if (!neighbor.IsEmpty && !neighbor.CurrentHexStack.IsUnityNull())
                        {
                            HexagonPoolObject newNeighborTop = neighbor.GetTopHexagon();
                            HexagonPoolObject ourNewTop = GetTopHexagon();

                            if (newNeighborTop.SoAllySoldierData == ourNewTop.SoAllySoldierData)
                            {
                                ProcessNextTransfer(matchingNeighbors, index);
                            }
                            else
                            {
                                ProcessNextTransfer(matchingNeighbors, index + 1);
                            }
                        }
                        else
                        {
                            ProcessNextTransfer(matchingNeighbors, index + 1);
                        }
                    });
                }
                else
                {
                    ProcessNextTransfer(matchingNeighbors, index + 1);
                }
            }
            else
            {
                ProcessNextTransfer(matchingNeighbors, index + 1);
            }
        }
        else
        {
            if (CurrentHexStack != null && !CurrentHexStack.IsUnityNull() && CurrentHexStack.hexPoolObjects.Count > 0)
            {
                int topIndex = CurrentHexStack.hexPoolObjects.Count - 1;
                StartTransferSequence(CurrentHexStack.hexPoolObjects[topIndex]);
            }
        }
    }

    private int transferCount = 0;
    
    private void TransferSingleHexagonWithCallback(HexGround neighbor, HexagonPoolObject hexagonToTransfer, Action onComplete)
    {
        transferCount++;
        
        int neighborTopIndex = neighbor.CurrentHexStack.hexPoolObjects.Count - 1;
        HexagonPoolObject topHexagon = neighbor.CurrentHexStack.hexPoolObjects[neighborTopIndex];

        Vector3 originalPosition = topHexagon.transform.position;
        
        float offsetY = 0.05f;
        
        Vector3 startOffset = new Vector3(
            0.02f * (transferCount % 3 - 1),
            offsetY + 0.015f * transferCount,
            0.01f * (transferCount % 2));
        
        Vector3 startPosition = originalPosition + startOffset;
        Quaternion originalRotation = topHexagon.transform.rotation;

        if (neighbor.CurrentHexStack.stackIconSprite != null)
        {
            neighbor.CurrentHexStack.stackIconSprite.enabled = false;
        }

        neighbor.CurrentHexStack.hexPoolObjects.RemoveAt(neighborTopIndex);

        if (neighbor.CurrentHexStack.hexPoolObjects.Count == 0)
        {
            neighbor.IsEmpty = true;
            
            HexStack stackToRemove = neighbor.CurrentHexStack;
            
            if (stackToRemove != null)
            {
                // HexSpawnController'dan da stack'i kaldır
                if (stackToRemove.HexSpawnController != null)
                {
                    stackToRemove.HexSpawnController.RemoveStackFromList(stackToRemove);
                }
                
                Destroy(stackToRemove.gameObject);
            }
            
            neighbor.CurrentHexStack = null;
        }
        else
        {
            UpdateStackIconSprite(neighbor.CurrentHexStack);
        }

        int currentStackCount = CurrentHexStack.hexPoolObjects.Count;
        
        if (CurrentHexStack.stackIconSprite != null)
        {
            CurrentHexStack.stackIconSprite.enabled = false;
        }
        
        CurrentHexStack.hexPoolObjects.Add(topHexagon);

        Vector3 localOffset = Vector3.zero;
        localOffset.y = 0.15f * currentStackCount;
        
        Vector3 finalWorldPosition = CurrentHexStack.transform.TransformPoint(localOffset);
        
        Vector3 midPoint = (startPosition + finalWorldPosition) / 2f;
        
        float distance = Vector3.Distance(startPosition, finalWorldPosition);
        
        float arcHeight = distance * 0.12f;
        midPoint.y += arcHeight;
        
        Vector3[] path = new Vector3[5];
        path[0] = startPosition;
        
        path[1] = Vector3.Lerp(startPosition, midPoint, 0.3f);
        path[1].y += distance * 0.04f;
        
        path[2] = midPoint;
        
        path[3] = Vector3.Lerp(midPoint, finalWorldPosition, 0.7f);
        path[3].y += distance * 0.04f;
        
        path[4] = finalWorldPosition;
        
        topHexagon.transform.SetParent(null);
        topHexagon.transform.position = startPosition;
        topHexagon.transform.rotation = originalRotation;
        
        Sequence transferSequence = DOTween.Sequence();
        
        transferSequence.Append(
            topHexagon.transform.DORotate(new Vector3(-2f, 0, 0), 0.1f).SetEase(Ease.InOutSine)
        );
        
        float animDuration = 0.3f;
        
        transferSequence.Append(
            topHexagon.transform.DOPath(path, animDuration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .SetOptions(false)
        );
        
        transferSequence.Join(
            DOTween.To(() => 0f, x => {
                float angle = x * 180f;
                topHexagon.transform.rotation = Quaternion.Euler(angle, 0, 0);
            }, 1f, animDuration).SetEase(Ease.Linear)
        );
        
        transferSequence.OnComplete(() => {
            Vector3 exactWorldPos = topHexagon.transform.position;
            
            topHexagon.transform.SetParent(CurrentHexStack.transform);
            
            topHexagon.transform.localPosition = localOffset;
            topHexagon.transform.localRotation = Quaternion.identity;
            
            UpdateStackIconSprite(CurrentHexStack);
            
            onComplete?.Invoke();
        });
    }
    
    private void UpdateStackIconSprite(HexStack hexStack)
    {
        if (hexStack == null || hexStack.hexPoolObjects.Count == 0 || hexStack.stackIconSprite == null)
            return;
        
        HexagonPoolObject topHexagon = hexStack.hexPoolObjects[hexStack.hexPoolObjects.Count - 1];
        
        hexStack.stackIconSprite.sprite = topHexagon.SoAllySoldierData.menuSprite;
        
        Vector3 spawnOffset = Vector3.zero;
        spawnOffset.z = -(0.15f * hexStack.hexPoolObjects.Count) - 0.075f;
        
        hexStack.stackIconSprite.transform.position = hexStack.transform.position + spawnOffset;
        
        hexStack.stackIconSprite.enabled = true;
    }

    private void CheckNeighborsForTransfer(HexagonPoolObject topHexagon)
    {
        StartTransferSequence(topHexagon);
    }

    private void TransferSingleHexagonFromNeighbor(HexGround neighbor, HexagonPoolObject hexagonToTransfer)
    {
        TransferSingleHexagonWithCallback(neighbor, hexagonToTransfer, null);
    }

    private void TransferHexagonFromNeighbor(HexGround neighbor, HexagonPoolObject hexagonToTransfer)
    {
        TransferSingleHexagonWithCallback(neighbor, hexagonToTransfer, null);
    }

    public HexagonPoolObject GetTopHexagon()
    {
        return CurrentHexStack.hexPoolObjects[^1];
    }

    public void OnHoverEnter()
    {
        meshRenderer.material = hoveredMaterial;
    }

    public void OnHoverExit()
    {
        meshRenderer.material = originalMaterial;
    }
}