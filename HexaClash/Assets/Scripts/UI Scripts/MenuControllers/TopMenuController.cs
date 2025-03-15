using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TopMenuController : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private TextMeshProUGUI moneyText; // TODO YAPILACAK
    [SerializeField] private List<string> moneyMulti = new();
    [SerializeField] private GameObject coin, money;
    
    private void Start()
    {
        settingsButton.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        EventManager.Instance.onSettingsOpened.Invoke();
    }
    
    private void SetMoneyText()
    {
        if (coin.activeSelf)
        {
            coin.transform.DORewind();
            coin.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);
        }
    
    
        if (money.activeSelf)
        {
            money.transform.DORewind();
            money.transform.DOPunchScale(Vector3.one, 0.5f, 2, 1);
        }
    
        int moneyDigit = GameManager.Instance.PlayerMoney.ToString().Length;
        int value = (moneyDigit - 1) / 3;
        if (value < 1)
        {
            moneyText.text = GameManager.Instance.PlayerMoney.ToString();
        }
        else
        {
            float temp = GameManager.Instance.PlayerMoney / Mathf.Pow(1000, value);
            moneyText.text = temp.ToString("F2") + " " + moneyMulti[value];
        }
    }
}
