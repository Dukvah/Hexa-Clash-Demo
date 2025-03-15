using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class GoHomePanel : MonoBehaviour
{
    [SerializeField] private Button yesButton, noButton;
    [SerializeField] private RectTransform frameRect;

    private void OnEnable()
    {
        OpenPanel();
    }

    private void Start()
    {
        yesButton.onClick.AddListener(GoHome);
        noButton.onClick.AddListener(ClosePanel);
    }

    private void GoHome()
    {
        EventManager.Instance.onMainMenuOpened.Invoke();
        gameObject.SetActive(false);
    }

    private void OpenPanel()
    {
        frameRect.localScale = Vector3.zero;
        frameRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
    }
    private void ClosePanel()
    {
        frameRect.localScale = Vector3.one;
        frameRect.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBounce)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
