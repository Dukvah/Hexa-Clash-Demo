using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Managers;

public class SettingsMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Sprite onHandleIcon, offHandleIcon;
    [SerializeField] private Slider soundSlider, musicSlider, vibrationSlider;
    [SerializeField] private Image soundHandleImage, musicHandleImage, vibrationHandleImage;
    [SerializeField] private GameObject soundSliderOnIconObj, soundSliderOffIconObj;
    [SerializeField] private GameObject musicSliderOnIconObj, musicSliderOffIconObj;
    [SerializeField] private GameObject vibrationSliderOnIconObj, vibrationSliderOffIconObj;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        closeButton.onClick.AddListener(Closing);
        SetAnimationProperties();
    }
    private void OnEnable()
    {
        Opening();
    }

    private void Start()
    {
        SliderInitializer();
    }

    #endregion

    #region Settings

    private void SliderInitializer()
    {
        soundSlider.onValueChanged.AddListener(SoundSliderValueChanged);
        musicSlider.onValueChanged.AddListener(MusicSliderValueChanged);
        vibrationSlider.onValueChanged.AddListener(VibrationSliderValueChanged);

        soundSlider.value = GameManager.Instance.GameSoundStatus == 1 ? 1 : 0;
        soundHandleImage.sprite = GameManager.Instance.GameSoundStatus == 1 ? onHandleIcon : offHandleIcon;
        soundSliderOnIconObj.SetActive(GameManager.Instance.GameSoundStatus == 1);
        soundSliderOffIconObj.SetActive(GameManager.Instance.GameSoundStatus != 1);
        
        musicSlider.value = GameManager.Instance.GameMusicStatus == 1 ? 1 : 0;
        musicHandleImage.sprite = GameManager.Instance.GameMusicStatus == 1 ? onHandleIcon : offHandleIcon;
        musicSliderOnIconObj.SetActive(GameManager.Instance.GameMusicStatus == 1);
        musicSliderOffIconObj.SetActive(GameManager.Instance.GameMusicStatus != 1);
        
        vibrationSlider.value = GameManager.Instance.GameVibrationStatus == 1 ? 1 : 0;
        vibrationHandleImage.sprite = GameManager.Instance.GameVibrationStatus == 1 ? onHandleIcon : offHandleIcon;
        vibrationSliderOnIconObj.SetActive(GameManager.Instance.GameVibrationStatus == 1);
        vibrationSliderOffIconObj.SetActive(GameManager.Instance.GameVibrationStatus != 1);
    }

    private void SoundSliderValueChanged(float newValue)
    {
        GameManager.Instance.GameSoundStatus = newValue == 0 ? 0 : 1;
        soundHandleImage.sprite = newValue == 0 ? offHandleIcon : onHandleIcon;
        soundSliderOnIconObj.SetActive(newValue != 0);
        soundSliderOffIconObj.SetActive(newValue == 0);
    }
    private void MusicSliderValueChanged(float newValue)
    {
        GameManager.Instance.GameMusicStatus = newValue == 0 ? 0 : 1;
        musicHandleImage.sprite = newValue == 0 ? offHandleIcon : onHandleIcon;
        musicSliderOnIconObj.SetActive(newValue != 0);
        musicSliderOffIconObj.SetActive(newValue == 0);
    }
    private void VibrationSliderValueChanged(float newValue)
    {
        GameManager.Instance.GameVibrationStatus = newValue == 0 ? 0 : 1;
        vibrationHandleImage.sprite = newValue == 0 ? offHandleIcon : onHandleIcon;
        vibrationSliderOnIconObj.SetActive(newValue != 0);
        vibrationSliderOffIconObj.SetActive(newValue == 0);
    }

    #endregion
    
    #region Opening & Closing  

    [Header("Opening & Closing")]
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform titleRectTransform, choicesRectTransform;

    private Vector2 titleOriginalPos, titleTargetPos;
    private Vector2 choicesOriginalPos, choicesTargetPos;
    
    private void SetAnimationProperties()
    {
        titleOriginalPos = titleRectTransform.anchoredPosition;
        choicesOriginalPos = choicesRectTransform.anchoredPosition;
        
        titleTargetPos = titleOriginalPos + new Vector2(-200f, 0f);
        choicesTargetPos = choicesOriginalPos + new Vector2(-200f, 0f);
    }
    private void Opening()
    {
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.DOFade(1f, 0.3f);

        titleRectTransform.anchoredPosition = titleTargetPos;
        titleRectTransform.DOAnchorPos(titleOriginalPos, .2f).SetEase(Ease.OutCubic).SetDelay(0.1f);
        
        choicesRectTransform.anchoredPosition = choicesTargetPos;
        choicesRectTransform.DOAnchorPos(choicesOriginalPos, .2f).SetEase(Ease.OutCubic);
    }
    private void Closing()
    {
        closeButton.interactable = false;
        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.DOFade(0f, 0.3f);
        
        titleRectTransform.anchoredPosition = titleOriginalPos;
        titleRectTransform.DOAnchorPos(titleTargetPos, .2f).SetEase(Ease.OutCubic);

        choicesRectTransform.anchoredPosition = choicesOriginalPos;
        choicesRectTransform.DOAnchorPos(choicesTargetPos, .2f).SetEase(Ease.OutCubic).SetDelay(0.1f)
            .OnComplete(() =>
            {
                titleRectTransform.anchoredPosition = titleOriginalPos;
                choicesRectTransform.anchoredPosition = choicesOriginalPos;
                closeButton.interactable = true;
                gameObject.SetActive(false);
            });
    }
    
    #endregion
}
