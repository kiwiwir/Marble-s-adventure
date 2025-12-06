using UnityEngine;

public class UIManager : MonoBehaviour, IMenu
{
    [SerializeField] private CanvasGroup questMenu;
    [SerializeField] private CanvasGroup diaryMenu;
    [SerializeField] private QuestLogUI questLogUI;

    private bool questOpen = false;
    private bool isFading = false;

    private void Start()
    {
        GlobalMenuManager.Instance.Register(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Toggle();
        }
    }

    // ————————————————————————
    //  NOWA METODA: TOGGLE()
    // ————————————————————————
    public void Toggle()
    {
        if (isFading) return;

        if (!questOpen)
            GlobalMenuManager.Instance.RequestOpen(this);
        else
            GlobalMenuManager.Instance.RequestClose(this);
    }


    public void Open()
    {
        // Zamknij inne menu (np. journal)
        SetMenuState(diaryMenu, false);

        SetMenuState(questMenu, true);
        questOpen = true;

        if (questLogUI != null)
        {
            questLogUI.UpdateAllQuestProgress();
            questLogUI.RefreshQuestList();
        }
    }

    public void Close()
    {
        SetMenuState(questMenu, false);
        questOpen = false;
    }

    public void CloseInstant()
    {
        SetMenuState(questMenu, false);
        questOpen = false;
    }

    public bool IsOpen => questOpen;

    private void SetMenuState(CanvasGroup group, bool isActive)
    {
        if (group == null) return;

        group.alpha = isActive ? 1 : 0;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;
    }
}
