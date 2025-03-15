using Base;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Pool hexagonPool;
        
        private SOLevel currentLevelData;
        private GameObject currentLevelObject;
        
        public Pool HexagonPool => hexagonPool;
        
        private float playerMoney;
        public float PlayerMoney
        {
            get => playerMoney;
            set
            {
                playerMoney = value;
                EventManager.Instance.onMoneyChange.Invoke();
            }
        }

        public int GameSoundStatus { get; set; }
        public int GameMusicStatus { get; set; }
        public int GameVibrationStatus { get; set; }


        private void Awake()
        {
            LoadData();
            EventManager.Instance.onNewLevelOpened.AddListener(OpenNewLevel);
            EventManager.Instance.onCurrentLevelClosed.AddListener(CloseLevel);
        }

        private void OpenNewLevel(SOLevel newLevel)
        {
            if (currentLevelData == null)
            {
                EventManager.Instance.gameStart.Invoke();
                currentLevelData = newLevel;
                currentLevelObject = Instantiate(currentLevelData.levelPrefab);
            }
        }

        private void CloseLevel()
        {
            currentLevelData = null;
            Destroy(currentLevelObject);
        }

        private void OnDisable()
        {
            SaveData();
        }

        private void LoadData()
        {
            playerMoney = PlayerPrefs.GetFloat("PlayerMoney", 0);
            GameSoundStatus = PlayerPrefs.GetInt("GameSoundStatus", 1);
            GameMusicStatus = PlayerPrefs.GetInt("GameMusicStatus", 1);
            GameVibrationStatus = PlayerPrefs.GetInt("GameVibrationStatus", 1);
        }

        private void SaveData()
        {
            PlayerPrefs.SetFloat("PlayerMoney", playerMoney);
            PlayerPrefs.SetInt("GameSoundStatus", GameSoundStatus);
            PlayerPrefs.SetInt("GameMusicStatus", GameMusicStatus);
            PlayerPrefs.SetInt("GameVibrationStatus", GameVibrationStatus);
        }
    }
}