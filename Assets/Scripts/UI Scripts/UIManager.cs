using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuBar;
    private bool isMenuActive;

    [SerializeField] private CanvasGroup diaryMenu;
    [SerializeField] private CanvasGroup questMenu;

    public void ToggleMenu(CanvasGroup target)
    {
        SetMenuState(diaryMenu, false);
        SetMenuState(questMenu, false);

        SetMenuState(target, true);
    }

    private void SetMenuState(CanvasGroup group, bool isActive)
    {
        group.alpha = isActive ? 1 : 0;
        group.interactable = isActive;
        group.blocksRaycasts = isActive;
    }

}
