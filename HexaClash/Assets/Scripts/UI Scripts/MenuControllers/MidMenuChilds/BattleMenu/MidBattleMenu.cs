using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Managers;
using ScriptableObjects;
using TMPro;

public class MidBattleMenu : MonoBehaviour
{
    private void Awake()
    {
        LevelsCarouselInitialize();
    }
    
    private void OnEnable()
    {
        Opening();
    }
    private void Start()
    {
        CalculateCarouselProperties();
        
        leftButton.onClick.AddListener(ScrollLeft);
        rightButton.onClick.AddListener(ScrollRight);
        playButton.onClick.AddListener(TryStartGame);
    }
    
    
    #region Select Level Carousel

    [Header("Select Level Carousel")]
    [SerializeField] private List<SOLevel> levels = new();
    [SerializeField] private LevelIcon levelIconPrefab;
    
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform scrollContent; 
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Button leftButton, rightButton, playButton;
    
    private float spacing;        
    private int elementCount;
    private int selectedLevelIndex = 0;
    
    private void LevelsCarouselInitialize()
    {
        foreach (var level in levels)
        {
            var levelIcon = Instantiate(levelIconPrefab, scrollContent.transform);
            bool isLevelOpen = PlayerPrefs.GetInt($"Level{level.levelNumber}IsOpen", 0) == 1;

            if (level.levelNumber == 1)
            {
                isLevelOpen = true;
                leftButton.gameObject.SetActive(false);
                playButton.interactable = true;
                levelNameText.text = $"1: {levels[selectedLevelIndex].levelName}";
            }
            
            levelIcon.IconInitializer(level.levelIcon, isLevelOpen);
        }
    }
    
    private void CalculateCarouselProperties()
    {
        elementCount = scrollContent.childCount;
    }
    
    private void ScrollLeft()
    {
        SetStatusScrollButtons(false);
        rightButton.gameObject.SetActive(true);
        
        selectedLevelIndex--;
        if (selectedLevelIndex == 0)
            leftButton.gameObject.SetActive(false);
        else
            leftButton.gameObject.SetActive(true);
        
        float currentPosition = scrollRect.horizontalNormalizedPosition;
        float step = 1f / (elementCount - 1);
        float newPosition = Mathf.Clamp01(currentPosition - step);
        
        DOTween.To(() => 
            scrollRect.horizontalNormalizedPosition, x 
            => scrollRect.horizontalNormalizedPosition = x, newPosition, .5f)
            .OnComplete(() => SetStatusScrollButtons(true));
        
        UpdateLevelStatus();
    }

    private void ScrollRight()
    {
        SetStatusScrollButtons(false);
        leftButton.gameObject.SetActive(true);
        
        selectedLevelIndex++;
        if (selectedLevelIndex == levels.Count - 1)
            rightButton.gameObject.SetActive(false);
        else
            rightButton.gameObject.SetActive(true);
        
        float currentPosition = scrollRect.horizontalNormalizedPosition;
        float step = 1f / (elementCount - 1);
        float newPosition = Mathf.Clamp01(currentPosition + step);
        
        DOTween.To(() => 
            scrollRect.horizontalNormalizedPosition, x 
            => scrollRect.horizontalNormalizedPosition = x, newPosition, .5f)
            .OnComplete(() => SetStatusScrollButtons(true));
        
        UpdateLevelStatus();
    }

    private void SetStatusScrollButtons(bool isOpen)
    {
        leftButton.interactable = isOpen;
        rightButton.interactable = isOpen;
    }

    private void UpdateLevelStatus()
    {
        levelNameText.text = $"{selectedLevelIndex+1}: {levels[selectedLevelIndex].levelName}";
        
        var isLevelOpen = PlayerPrefs.GetInt($"Level{levels[selectedLevelIndex].levelNumber}IsOpen", 0) == 1;
        playButton.interactable = isLevelOpen || selectedLevelIndex==0;
    }

    private void TryStartGame()
    {
        EventManager.Instance.onNewLevelOpened.Invoke(levels[selectedLevelIndex]);
    }
    
    #endregion

    #region Chest Open
    
    [Header("Chest Open")]
    [SerializeField] private Button normalChestOpen, epicChestOpen;

    #endregion
    
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
