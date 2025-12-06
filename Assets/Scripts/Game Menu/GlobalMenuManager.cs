using System.Collections.Generic;
using UnityEngine;

public class GlobalMenuManager : MonoBehaviour
{
    public static GlobalMenuManager Instance;

    private List<IMenu> registeredMenus = new List<IMenu>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Register(IMenu menu)
    {
        if (!registeredMenus.Contains(menu))
            registeredMenus.Add(menu);
    }

    public void RequestOpen(IMenu menuToOpen)
    {
        // Zamykamy wszystkie inne menu
        foreach (var menu in registeredMenus)
        {
            if (menu != menuToOpen)
                menu.CloseInstant();
        }

        // Otwieramy to właściwe
        menuToOpen.Open();
    }

    public void RequestClose(IMenu menuToClose)
    {
        menuToClose.Close();
    }
}
public interface IMenu
{
    void Open();
    void Close();
    void CloseInstant();
    bool IsOpen { get; }
}

