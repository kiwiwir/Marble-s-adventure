using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    private bool isMenuActive;

    [SerializeField] private CanvasGroup diaryMenu;
    [SerializeField] private CanvasGroup questMenu;
    [SerializeField] private QuestLogUI questLogUI; // Dodaj referencję


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
