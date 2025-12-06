using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    [SerializeField] private CanvasGroup diaryMenu;
    [SerializeField] private CanvasGroup questMenu;
    [SerializeField] private QuestLogUI questLogUI;

    private void Update()
    {
        // Obsługa klawisza Q do otwierania/zamykania questMenu
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool questIsOpen = questMenu.alpha > 0.9f;

            if (questIsOpen)
            {
                // Zamknij gdy jest otwarte
                SetMenuState(questMenu, false);
            }
            else
            {
                // Zamknij inne menu
                SetMenuState(diaryMenu, false);

                // Otwórz quest menu
                SetMenuState(questMenu, true);

                // Odświeżanie questów przy otwarciu
                if (questLogUI != null)
                {
                    questLogUI.UpdateAllQuestProgress();
                    questLogUI.RefreshQuestList();
                }
            }
        }
    }

    public void ToggleMenu(CanvasGroup target)
    {
        SetMenuState(diaryMenu, false);
        SetMenuState(questMenu, false);

        SetMenuState(target, true);

        // Jeśli otwieramy questMenu, odśwież questy
        if (target == questMenu && questLogUI != null)
        {
            questLogUI.UpdateAllQuestProgress();  // Aktualizuje progres wszystkich questów
            questLogUI.RefreshQuestList();        // Odświeża sloty w UI
        }
    }


    private void SetMenuState(CanvasGroup group, bool isActive)
    {
        group.alpha = isActive ? 1 : 0;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;
    }

}
