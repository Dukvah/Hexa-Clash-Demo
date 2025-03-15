using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private HexSpawnController hexSpawnController;

    private int currentStage = 0;
    
    private void OnEnable()
    {
        StartNewStage();
        EventManager.Instance.onMainMenuOpened.AddListener(() => Invoke(nameof(CloseLevel), 1f));
    }


    private void StartNewStage()
    {
        hexSpawnController.SetNewStage(currentStage);
    }
    private void CloseLevel()
    {
        EventManager.Instance.onCurrentLevelClosed.Invoke();
        Destroy(gameObject);
    }
}
