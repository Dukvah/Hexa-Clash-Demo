using UnityEngine;
using UnityEngine.UI;

public class LevelIcon : MonoBehaviour
{
    public Image levelIcon;
    public GameObject lockObject;
    public bool IsOpen { get; private set; }

    public void IconInitializer(Sprite icon, bool isOpen)
    {
        levelIcon.sprite = icon;
        IsOpen = isOpen;
        lockObject.SetActive(!isOpen);
    }
}
