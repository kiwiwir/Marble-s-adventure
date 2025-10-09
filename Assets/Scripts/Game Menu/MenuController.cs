using UnityEngine;

public class MenuController : MonoBehaviour
{
    public CanvasGroup menuCanvasGroup;
    public TabController tabController;
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
        if (Input.GetButtonDown("ToggleMenu") && !isFading)
        {
            if (!isMenuOpen)
            {
                //AudioManager.Play("Menu_In");
                StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 0f, 1f));
                isMenuOpen = true;
                tabController.ActiveTab(0);

                Time.timeScale = 0f;
            }
            else
            {
                AudioManager.Play("Menu_Out");
                StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 1f, 0f));
                isMenuOpen = false;

                Time.timeScale = 1f;
            }
        }
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
}

