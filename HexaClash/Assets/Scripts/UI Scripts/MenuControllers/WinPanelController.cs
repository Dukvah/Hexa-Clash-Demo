using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelController : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    private void Awake()
    {
        closeBtn.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        Opening();
    }

    private void ClosePanel()
    {
        EventManager.Instance.onMainMenuOpened.Invoke();
    }
    
    #region Opening

    [Header("Opening")]
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform tapToPlayText;
    [SerializeField] private List<RectTransform> floatingRectTransforms = new();
    
    private List<Vector2> originalPoses = new();
    private List<Vector2> startPoses = new();
    private bool posesAssigned;

    private void Opening()
    {
        SetPositions();
        
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.DOFade(1, 0.3f);
        var delay = .1f;
        for (int i = 0; i < floatingRectTransforms.Count; i++)
        {
            floatingRectTransforms[i].anchoredPosition = startPoses[i];
            floatingRectTransforms[i].DOAnchorPos(originalPoses[i], .1f).SetEase(Ease.OutCubic).SetDelay(delay);
            delay += .05f;
        }

        tapToPlayText.DOKill();
        tapToPlayText.localScale = Vector3.zero;
        tapToPlayText.DOScale(1f, 1.5f).SetEase(Ease.OutBack).SetDelay(delay).OnComplete(() =>
        {
            tapToPlayText.DOScale(1.2f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine); 
        });
    }
    
    private void SetPositions()
    {
        if (posesAssigned) return;

        foreach (var rectTransform in floatingRectTransforms)
        {
            originalPoses.Add(rectTransform.anchoredPosition);
            startPoses.Add(rectTransform.anchoredPosition + new Vector2(-100f, 10f));
        }
        
        posesAssigned = true;
    }

    #endregion
}
