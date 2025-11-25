using UnityEngine;

public class MenuController : MonoBehaviour
{
    public CanvasGroup menuCanvasGroup;
    public TabController tabController;
    public PauseController pauseController;
    private bool isMenuOpen = false;
    public float fadeSpeed = 3f; // prędkość animacji
    private bool isFading = false;

    void Start()
    {
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        // --- Otwieranie / zamykanie menu ---
        if (Input.GetButtonDown("ToggleMenu") && !isFading)
        {
            if (!isMenuOpen)
            {
                OpenMenu(0); // domyślnie strona 0 (np. statystyki)
            }
            else
            {
                CloseMenu();
            }
        }

        // --- Otwieranie menu od razu z mapą ---
        if (Input.GetButtonDown("ToggleMap") && !isFading)
        {
            if (!isMenuOpen)
            {
                OpenMenu(2); // jeśli menu jest zamknięte → otwórz z mapą
            }
            else
            {
                tabController.ActiveTab(2); // jeśli otwarte → przełącz tylko stronę
            }
        }
    }

    public void ToggleMenuButton()
    {
        if (!isMenuOpen && !isFading)
        {
            OpenMenu(0);
        }
        else if (!isFading)
        {
            CloseMenu();
        }
    }


    private void OpenMenu(int tabIndex)
    {
        // Zamknij pauzę, jeśli aktywna
        if (pauseController != null && pauseController.IsPaused)
        {
            pauseController.ClosePauseInstant();
        }

        StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 0f, 1f));
        isMenuOpen = true;
        tabController.ActiveTab(tabIndex);

        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        AudioManager.Play("Menu_Out");
        StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 1f, 0f));
        isMenuOpen = false;
        Time.timeScale = 1f;
    }

    public void CloseMenuInstant()
    {
        StopAllCoroutines();
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
        isMenuOpen = false;
        Time.timeScale = 1f;
    }

    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end)
    {
        isFading = true;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.unscaledDeltaTime * fadeSpeed; // unscaled, by działało przy wstrzymanym czasie
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed);
            yield return null;
        }

        canvasGroup.alpha = end;

        // Włączanie/wyłączanie interakcji zależnie od widoczności
        bool visible = end > 0.9f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;

        isFading = false;
    }

    public bool IsMenuOpen => isMenuOpen;
}
