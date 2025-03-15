using System;
using System.Collections;
using System.Globalization;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
    public RectTransform parentRect;
    [SerializeField] private SOAllySoldierData sOAllySoldierData;
    
    [SerializeField] private Image heroStandSprite;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject onButtonImage, offButtonImage;
    
    [SerializeField] private GameObject lockObject, cardObject;
    
    [SerializeField] private CanvasGroup cardCanvasGroup;


    private int levelUpCost;
    private Vector2 originalPos, startPos;
    private bool posesAssigned;
    

    public void SetCard(float delay)
    {
        SetPositions();
        sOAllySoldierData.LoadSoldierProperties();
        
        if (sOAllySoldierData.IsOpen)
        {
            lockObject.SetActive(false);
            cardObject.SetActive(true);

            atkText.text = sOAllySoldierData.AttackDamage.ToString(CultureInfo.InvariantCulture);
            hpText.text = sOAllySoldierData.HealthPoint.ToString(CultureInfo.InvariantCulture);
            levelText.text = $"Lv. {sOAllySoldierData.level.ToString()}";
            heroStandSprite.sprite = sOAllySoldierData.menuSprite;
            heroStandSprite.SetNativeSize();
            
            levelUpCost = 10 * sOAllySoldierData.level;
            priceText.text = levelUpCost.ToString();
            
            if (GameManager.Instance.PlayerMoney >= levelUpCost)
            {
                levelUpButton.interactable = true;
                onButtonImage.SetActive(true);
                offButtonImage.SetActive(false);
                priceText.color = Color.white;
            }
            else
            {
                levelUpButton.interactable = false;
                onButtonImage.SetActive(false);
                offButtonImage.SetActive(true);
                priceText.color = Color.red;
            }
        }
        else
        {
            lockObject.SetActive(true);
            cardObject.SetActive(false);
        }
        
        cardCanvasGroup.alpha = 0f;
        cardCanvasGroup.DOFade(1, 0.3f).SetDelay(delay);
        
        
        parentRect.anchoredPosition = startPos;
        parentRect.DOAnchorPos(originalPos, .15f).SetEase(Ease.OutCubic).SetDelay(delay);
    }

    private void SetPositions()
    {
        if (posesAssigned) return;
        
        originalPos = parentRect.anchoredPosition;
        startPos = originalPos + new Vector2(-100f, 10f);
        posesAssigned = true;
    }
}
