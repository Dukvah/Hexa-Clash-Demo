using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Managers;

public class BottomMenuController : MonoBehaviour
{
    [SerializeField] private List<SectionButtons> sectionButtons = new();
    [SerializeField] private Selector selector;
    
    private int currentSectionID = 1;

    private int sectionIconsOriginalPosY = 0;
    private int sectionIconsSelectedPosY = 150;

    private void Start()
    {
        ButtonInitializer();
    }

    private void ButtonInitializer()
    {
        for (int i = 0; i < sectionButtons.Count; i++)
        {
            var temp = i;
            sectionButtons[temp].button.onClick.AddListener(() => SelectNewSection(sectionButtons[temp].id));
        }
    }
    
    private void SelectNewSection(int sectionId)
    {
        if (sectionId != currentSectionID)
        {
            MoveIcon(sectionButtons[currentSectionID].iconRect, sectionIconsOriginalPosY);
            MoveIcon(sectionButtons[sectionId].iconRect, sectionIconsSelectedPosY);
            MoveSelector(sectionId);
            currentSectionID = sectionId;
            EventManager.Instance.onMenuSectionChanged.Invoke(sectionId);
        }
        else
        {
            TriggerSelectedIcon();
        }
    }
    
    private void MoveSelector(int targetSectionId)
    {
        selector.textCanvasGroup.DOFade(0, 0.1f).OnComplete(() =>
        {
            selector.nameText.text = sectionButtons[targetSectionId].name;
            selector.textCanvasGroup.DOFade(1, 0.1f);
        });
        
        Vector2 targetPos = new Vector2(sectionButtons[targetSectionId].buttonRect.anchoredPosition.x,selector.selectorRect.anchoredPosition.y);
        selector.selectorRect.DOAnchorPos(targetPos, .2f).SetEase(Ease.OutCubic);
    }
    private void MoveIcon(RectTransform targetRect, float offset)
    {
        Vector2 targetVector = new Vector2(targetRect.anchoredPosition.x, offset);
        targetRect.DOAnchorPos(targetVector,.1f, true);
    }

    private void TriggerSelectedIcon()
    {
        var temp = sectionButtons[currentSectionID].iconRect;
        var originalPos = new Vector2(temp.anchoredPosition.x, sectionIconsSelectedPosY);
        temp.DOKill();
        temp.DOAnchorPos(temp.anchoredPosition + new Vector2(0, -10f), 0.05f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            temp.DOAnchorPos(originalPos, 0.05f)
                .SetEase(Ease.InQuad); 
        });
    }
}

[Serializable]
public struct SectionButtons
{
    public int id;
    public string name;
    public Button button;
    public RectTransform buttonRect;
    public RectTransform iconRect;
}

[Serializable]
public struct Selector
{
    public RectTransform selectorRect;
    public CanvasGroup textCanvasGroup;
    public TextMeshProUGUI nameText;
}