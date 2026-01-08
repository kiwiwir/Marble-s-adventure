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

    public void Unregister(IMenu menu)
    {
        registeredMenus.Remove(menu);
    }

    public void RequestOpen(IMenu menuToOpen)
    {
        if (GameManager.Instance.DialogueManager != null &&
            GameManager.Instance.DialogueManager.isDialogueActive)
            return;

        registeredMenus.RemoveAll(m => m == null);

        foreach (var menu in registeredMenus)
        {
            if (menu != menuToOpen)
                menu.CloseInstant();
        }

        menuToOpen.Open();
    }

    public void RequestClose(IMenu menuToClose)
    {
        registeredMenus.RemoveAll(m => m == null);
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

