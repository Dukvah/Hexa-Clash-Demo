using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MidHeroesMenu : MonoBehaviour
{
    [SerializeField] private CharacterInfoPanel characterInfoPanel;
    [SerializeField] private List<HeroStand> heroStands = new();

    private void Awake()
    {
        HeroStandsInitializer();
    }

    private void OnEnable()
    {
        Opening();
    }
    
    private void HeroStandsInitializer()
    {
        foreach (var heroStand in heroStands)
        {
            heroStand.SetStand(OpenCharacterInfoPanel);
            heroStand.parentRect.DOAnchorPosY(heroStand.parentRect.anchoredPosition.y + Random.Range(7.5f, 10f),
                    Random.Range(1f, 1.5f)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine); 
        }
    }

    private void OpenCharacterInfoPanel(SOAllySoldierData soldierData)
    {
        characterInfoPanel.AllySoldierData = soldierData;
        characterInfoPanel.gameObject.SetActive(true);
    }
    
    #region Opening

    [Header("Opening")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform floatingRectTransform;

    private void Opening()
    {
        backgroundImage.sprite = backgroundSprite;
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.DOFade(1, 0.3f);

        var originalPos = floatingRectTransform.anchoredPosition;
        var startPos = originalPos + new Vector2(-200f, 0f);
        
        floatingRectTransform.anchoredPosition = startPos;
        floatingRectTransform.DOAnchorPos(originalPos, .4f).SetEase(Ease.OutCubic);
    }
    

    #endregion
}
[Serializable]
public struct HeroStand
{
    public SOAllySoldierData sOAllySoldierData;
    public Button openDetailsButton;
    public Image heroStandSprite;
    public TextMeshProUGUI levelText;
    public List<GameObject> starObjects;
    public RectTransform parentRect;
    
    public void SetStand(UnityAction<SOAllySoldierData> clickAction)
    {
        openDetailsButton.onClick.RemoveAllListeners();
        sOAllySoldierData.LoadSoldierProperties();
        
        if (sOAllySoldierData.IsOpen)
        {
            var data = sOAllySoldierData;
            openDetailsButton.onClick.AddListener(() => clickAction(data));
            starObjects[0].transform.parent.gameObject.SetActive(true);
            levelText.text = $"Lv. {sOAllySoldierData.level.ToString()}";
            heroStandSprite.sprite = sOAllySoldierData.menuSprite;
            heroStandSprite.SetNativeSize();
        }
    }
    
    private void SetStars(int starCount)
    {
        for (int i = 0; i < starCount; i++)
        {
            starObjects[i].SetActive(true);
        }
    }
}

