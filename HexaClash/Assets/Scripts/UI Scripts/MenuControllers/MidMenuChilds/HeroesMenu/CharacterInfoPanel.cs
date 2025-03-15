using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Managers;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    public SOAllySoldierData AllySoldierData { get; set; }
    
    [SerializeField] private List<Sprite> soldierTypeIcons = new();
    [SerializeField] private Image soldierIcon;
    [SerializeField] private TextMeshProUGUI soldierName;
    [SerializeField] private TextMeshProUGUI soldierAttack;
    [SerializeField] private TextMeshProUGUI soldierHealth;
    [SerializeField] private TextMeshProUGUI soldierLevel;
    [SerializeField] private TextMeshProUGUI soldierRarity;
    [SerializeField] private Image soldierTypeIcon;

    [SerializeField] private Image levelUpButtonImage;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Sprite enabledBuyButton, disabledBuyButton;
    [SerializeField] private TextMeshProUGUI levelUpCostText;

    private int levelUpCost;
    
    #region Unity Functions

    private void Awake()
    {
        closeButton.onClick.AddListener(Closing);
        levelUpButton.onClick.AddListener(LevelUpSoldier);
        SetAnimationProperties();
    }
    private void OnEnable()
    {
        SetCardProperties();
        Opening();
    }

    private void SetCardProperties()
    {
        soldierName.text = AllySoldierData.soldierName;
        soldierAttack.text = AllySoldierData.AttackDamage.ToString(CultureInfo.InvariantCulture);
        soldierHealth.text = AllySoldierData.HealthPoint.ToString(CultureInfo.InvariantCulture);
        soldierLevel.text = AllySoldierData.level.ToString();
        soldierRarity.text = AllySoldierData.soldierRarity.ToString();
        soldierTypeIcon.sprite = soldierTypeIcons[(int)AllySoldierData.soldierType];
        soldierIcon.sprite = AllySoldierData.menuSprite;

        levelUpCost = 10 * AllySoldierData.level;
        levelUpCostText.text = levelUpCost.ToString();

        if (GameManager.Instance.PlayerMoney >= levelUpCost)
        {
            levelUpButton.interactable = true;
            levelUpButtonImage.sprite = enabledBuyButton;
            levelUpCostText.color = Color.white;
        }
        else
        {
            levelUpButton.interactable = false;
            levelUpButtonImage.sprite = disabledBuyButton;
            levelUpCostText.color = Color.red;
        }
    }

    private void LevelUpSoldier()
    {
        if (GameManager.Instance.PlayerMoney >= levelUpCost)
        {
            AllySoldierData.LevelUp();
            SetCardProperties();
        }
    }

    #endregion
    
    #region Opening & Closing  

    [Header("Opening & Closing")]
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform topRectTransform, bottomRectTransform;

    private Vector2 titleOriginalPos, titleTargetPos;
    private Vector2 choicesOriginalPos, choicesTargetPos;
    
    private void SetAnimationProperties()
    {
        titleOriginalPos = topRectTransform.anchoredPosition;
        choicesOriginalPos = bottomRectTransform.anchoredPosition;
        
        titleTargetPos = titleOriginalPos + new Vector2(-200f, 0f);
        choicesTargetPos = choicesOriginalPos + new Vector2(-200f, 0f);
    }
    private void Opening()
    {
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.DOFade(1f, 0.3f);

        topRectTransform.anchoredPosition = titleTargetPos;
        topRectTransform.DOAnchorPos(titleOriginalPos, .2f).SetEase(Ease.OutCubic).SetDelay(0.1f);
        
        bottomRectTransform.anchoredPosition = choicesTargetPos;
        bottomRectTransform.DOAnchorPos(choicesOriginalPos, .2f).SetEase(Ease.OutCubic);
    }
    private void Closing()
    {
        closeButton.interactable = false;
        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.DOFade(0f, 0.3f);
        
        topRectTransform.anchoredPosition = titleOriginalPos;
        topRectTransform.DOAnchorPos(titleTargetPos, .2f).SetEase(Ease.OutCubic);

        bottomRectTransform.anchoredPosition = choicesOriginalPos;
        bottomRectTransform.DOAnchorPos(choicesTargetPos, .2f).SetEase(Ease.OutCubic).SetDelay(0.1f)
            .OnComplete(() =>
            {
                topRectTransform.anchoredPosition = titleOriginalPos;
                bottomRectTransform.anchoredPosition = choicesOriginalPos;
                closeButton.interactable = true;
                gameObject.SetActive(false);
            });
    }
    
    #endregion
}
