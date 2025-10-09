using UnityEngine;

public class PauseController : MonoBehaviour
{
    public CanvasGroup pauseCanvasGroup;
    public MenuController menuController;
    public float fadeSpeed = 3f;
    private bool isPaused = false;
    private bool isFading = false;

    void Start()
    {
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("TogglePause") && !isFading)
        {
            if (!isPaused)
            {
                // Jeśli menu jest otwarte, zamknij je
                if (menuController != null && menuController.IsMenuOpen)
                {
                    menuController.CloseMenuInstant();
                }

                // Odtwarzanie dźwięku otwierania pauzy
                AudioManager.Play("Menu_In");

                StartCoroutine(FadeCanvasGroup(pauseCanvasGroup, 0f, 1f));
                isPaused = true;
                Time.timeScale = 0f;
            }
            else
            {
                // Odtwarzanie dźwięku zamykania pauzy
                AudioManager.Play("Menu_Out");

                StartCoroutine(FadeCanvasGroup(pauseCanvasGroup, 1f, 0f));
                isPaused = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void ClosePauseInstant()
    {
        StopAllCoroutines();
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
        isPaused = false;
        Time.timeScale = 1f;
    }

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

        // Ustaw interakcje w zależności od widoczności
        bool visible = end > 0.9f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;

        isFading = false;
    }

    public bool IsPaused => isPaused;
}
