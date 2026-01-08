using UnityEngine;

public class PauseController : MonoBehaviour, IMenu
{
    public CanvasGroup pauseCanvasGroup;
    public MenuController menuController;
    public float fadeSpeed = 3f;

    private bool isPaused = false;
    private bool isFading = false;

    void Start()
    {
        GlobalMenuManager.Instance.Register(this);

        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("TogglePause") && !isFading)
        {
            // Pauza używa teraz systemu globalnego
            if (!isPaused)
                GlobalMenuManager.Instance.RequestOpen(this);
            else
                GlobalMenuManager.Instance.RequestClose(this);
        }
    }


    // ——————————————————————————
    //      IMPLEMENTACJA IMenu
    // ——————————————————————————

    public void Open()
    {
        // Zamknij Diary Menu jeśli otwarte
        if (menuController != null && menuController.IsMenuOpen)
            menuController.CloseInstant();

        // Dźwięk otwarcia
        AudioManager.Play("Menu_In");

        StartCoroutine(FadeCanvasGroup(pauseCanvasGroup, 0f, 1f));
        isPaused = true;

        Time.timeScale = 0f;
    }

    public void Close()
    {
        // Dźwięk zamykania
        AudioManager.Play("Menu_Out");

        StartCoroutine(FadeCanvasGroup(pauseCanvasGroup, 1f, 0f));
        isPaused = false;

        Time.timeScale = 1f;
    }

    public void CloseInstant()
    {
        StopAllCoroutines();
        isFading = false;

        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;

        isPaused = false;
        Time.timeScale = 1f;
    }

    public bool IsOpen => isPaused;
    public bool IsPaused => isPaused;


    // ——————————————————————————
    //      FADE ANIMACJA
    // ——————————————————————————

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
}