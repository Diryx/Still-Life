using UnityEngine;
using UnityEngine.UI;

public class MenuSwitcher : ButtonParent
{
    [SerializeField] private SwitchMenu _switchMenu;
    [SerializeField] private GameObject[] _menu;

    private enum SwitchMenu
    { 
        Menu_0 = 0,
        Menu_1 = 1,
        Menu_2 = 2,
        Menu_3 = 3,
        Menu_4 = 4,
        Menu_5 = 5
    }

    protected override void ButtonAction()
    {
        base.ButtonAction();

        foreach (GameObject menu in _menu)
            menu.SetActive(false);

        int index = (int)_switchMenu;
        if (index >= 0 && index < _menu.Length)
        _menu[index].SetActive(true);
    }
}