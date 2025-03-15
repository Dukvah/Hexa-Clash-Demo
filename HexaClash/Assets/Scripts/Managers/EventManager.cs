using Base;
using Base.PoolSystem.PoolTypes.Abstracts;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class EventManager : Singleton<EventManager>
    {
        [HideInInspector] public UnityEvent gameStart = new();
        [HideInInspector] public UnityEvent levelSuccess = new();
        [HideInInspector] public UnityEvent levelFail = new();
        [HideInInspector] public UnityEvent<SOLevel> onNewLevelOpened = new();
        [HideInInspector] public UnityEvent onCurrentLevelClosed = new();
        [HideInInspector] public UnityEvent onMainMenuOpened = new();
        [HideInInspector] public UnityEvent onMoneyChange = new();

        [HideInInspector] public UnityEvent<int> onBattlePreparationsStart = new();
        [HideInInspector] public UnityEvent onStackPlaced = new();
        
        #region UI Menu Events

        [HideInInspector] public UnityEvent<int> onMenuSectionChanged = new();
        [HideInInspector] public UnityEvent onSettingsOpened = new();
        
        #endregion

        #region Pool Events

        [HideInInspector] public UnityEvent<PoolObject> returnHexagonPool = new();

        #endregion
        
        
    }
}
