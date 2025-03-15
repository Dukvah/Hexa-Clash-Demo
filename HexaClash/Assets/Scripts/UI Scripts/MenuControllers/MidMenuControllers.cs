using System.Collections.Generic;
using Managers;
using UnityEngine;

public class MidMenuControllers : MonoBehaviour
{
    [SerializeField] private List<GameObject>  subMenus = new();

    private void Start()
    {
        EventManager.Instance.onMenuSectionChanged.AddListener(OpenSelectedSubMenu);
    }

    private void OpenSelectedSubMenu(int selectedIndex)
    {
        CloseAllSubMenus();
        subMenus[selectedIndex].gameObject.SetActive(true);
    }

    private void CloseAllSubMenus()
    {
        foreach (var subMenu in subMenus)
        {
            subMenu.SetActive(false);
        }
    }
}
