using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel, losePanel, inGamePanel, settingsPanel, menuPanel;
        [SerializeField] private CanvasGroup transitionPanel;

        private void OnEnable()
        {
            EventManager.Instance.gameStart.AddListener(() => ShowPanel(inGamePanel));
            EventManager.Instance.levelSuccess.AddListener(() => ShowPanel(winPanel));
            EventManager.Instance.levelFail.AddListener(() => ShowPanel(losePanel));
            EventManager.Instance.onMainMenuOpened.AddListener(() => ShowPanel(menuPanel, true));
            EventManager.Instance.onSettingsOpened.AddListener(OpenSettingsPanel);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.gameStart.RemoveAllListeners();
                EventManager.Instance.levelSuccess.RemoveAllListeners();
                EventManager.Instance.levelFail.RemoveAllListeners();
                EventManager.Instance.onSettingsOpened.RemoveAllListeners();
            }
        }
        
        private void ShowPanel(GameObject panel, bool transitionScreen = false)
        {
            if (transitionScreen)
            {
                transitionPanel.alpha = 0f;
                transitionPanel.DOFade(1, 0.3f).OnComplete(() =>
                {
                    OpenPanel(panel);
                    transitionPanel.DOFade(0, 0.3f).SetDelay(0.2f);
                });
            }
            else
            {
                OpenPanel(panel);
            }
        }

        private void OpenPanel(GameObject panel)
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);
            inGamePanel.SetActive(false);
            settingsPanel.SetActive(false);
            menuPanel.SetActive(false);
            
            panel.SetActive(true);
        }

        private void OpenSettingsPanel()
        {
            settingsPanel.SetActive(true);
        }
    }
}