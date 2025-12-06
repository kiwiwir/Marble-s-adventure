using UnityEngine;

public class MenuController : MonoBehaviour, IMenu
{
    public CanvasGroup menuCanvasGroup;
    public TabController tabController;
    public PauseController pauseController;

    private bool isMenuOpen = false;
    private bool isFading = false;

    public float fadeSpeed = 3f;

    void Start()
    {
        // Rejestracja w globalnym zarządcy menu
        GlobalMenuManager.Instance.Register(this);

        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        // ToggleMenu (TAB)
        if (Input.GetButtonDown("ToggleMenu") && !isFading)
        {
            /*if (!isMenuOpen)
                GlobalMenuManager.Instance.RequestOpen(this);
            else
                GlobalMenuManager.Instance.RequestClose(this);*/
            Toggle();
        }

        // ToggleMap (M)
        if (Input.GetButtonDown("ToggleMap") && !isFading)
        {
            if (!isMenuOpen)
            {
                // Otwieramy całe Marble Menu
                GlobalMenuManager.Instance.RequestOpen(this);

                // Po prostu przełączamy zakładkę
                tabController.ActiveTab(2);
            }
            else
            {
                tabController.ActiveTab(2);
            }
        }
    }
    public void Toggle()
    {
        if (isFading) return;

        if (!isMenuOpen)
            GlobalMenuManager.Instance.RequestOpen(this);
        else
            GlobalMenuManager.Instance.RequestClose(this);
    }


    // ————————————————————————
    //  IMPLEMENTACJA IMenu
    // ————————————————————————

    public void Open()
    {
        // Jeśli pauza jest otwarta → zamknij natychmiast
        if (pauseController != null && pauseController.IsPaused)
            pauseController.Close();

        StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 0f, 1f));
        isMenuOpen = true;

        // Domyślnie otwieramy stronę 0 przy wejściu
        tabController.ActiveTab(0);

        Time.timeScale = 0f;
    }

    public void Close()
    {
        StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 1f, 0f));
        isMenuOpen = false;

        Time.timeScale = 1f;
    }

    public void CloseInstant()
    {
        StopAllCoroutines();

        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        isMenuOpen = false;
        Time.timeScale = 1f;
    }

    public bool IsOpen => isMenuOpen;



    // ————————————————————————
    //  ANIMACJE I POMOCNICZE
    // ————————————————————————

    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end)
    {
        isFading = true;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.unscaledDeltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed);
            yield return null;
        }

        canvasGroup.alpha = end;

        bool visible = end > 0.9f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;

        isFading = false;
    }

    public bool IsMenuOpen => isMenuOpen;
}
