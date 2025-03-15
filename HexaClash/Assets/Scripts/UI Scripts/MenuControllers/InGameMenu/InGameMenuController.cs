using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuControoler : MonoBehaviour
{
    [SerializeField] private Button settingsButton, homeButton;
    [SerializeField] private GameObject goHomePanel;
    [SerializeField] private TextMeshProUGUI moneyText, gemText; // TODO YAPILACAK

    [SerializeField] private GameObject moveCountObject, battleProgressBar;
    [SerializeField] private TextMeshProUGUI moveCountText;
    [SerializeField] private Button startBattleButton;
    
    private int currentMoveCount;
    
    private void Awake()
    {
        settingsButton.onClick.AddListener(OpenSettings);
        homeButton.onClick.AddListener(OpenGoHomePanel);
        startBattleButton.onClick.AddListener(StartBattle);
        
        EventManager.Instance.onBattlePreparationsStart.AddListener(SetBattlePreparations);
        EventManager.Instance.onStackPlaced.AddListener(StackPlaced);
    }

    private void OpenBattleButton()
    {
        startBattleButton.gameObject.SetActive(true);
    }
    private void StartBattle()
    {
        startBattleButton.gameObject.SetActive(false);
        moveCountObject.SetActive(true);
        battleProgressBar.SetActive(false);
    }
    
    private void SetBattlePreparations(int moveCount)
    {
        currentMoveCount = moveCount;
        startBattleButton.gameObject.SetActive(false);
        battleProgressBar.SetActive(false);
        moveCountObject.SetActive(true);
        moveCountText.text = $"MOVES: {currentMoveCount}";
    }

    private void StackPlaced()
    {
        currentMoveCount--;
        moveCountText.text = $"MOVES: {currentMoveCount}";
        if (currentMoveCount == 0)
        {
            Invoke(nameof(OpenBattleButton), .5f);
        }
    }

    private void OpenSettings()
    {
        EventManager.Instance.onSettingsOpened.Invoke();
    }
    private void OpenGoHomePanel()
    {
        goHomePanel.SetActive(true);
    }
    
    
}
