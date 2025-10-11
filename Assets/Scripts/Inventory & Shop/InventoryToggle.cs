using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public CanvasGroup inventoryCanvasGroup; // ← dodaj ten komponent do panelu
    public float fadeSpeed = 3f;

    private bool isVisible = false;
    private bool isFading = false;

    void Start()
    {
        // Na start ukryj panel
        inventoryCanvasGroup.alpha = 0f;
        inventoryCanvasGroup.interactable = false;
        inventoryCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isFading)
        {
            if (!isVisible)
                StartCoroutine(FadeCanvasGroup(inventoryCanvasGroup, 0f, 1f, true));  // fade-in
            else
                StartCoroutine(FadeCanvasGroup(inventoryCanvasGroup, 1f, 0f, false)); // fade-out
        }
    }

    private System.Collections.IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, bool opening)
    {
        isFading = true;

        // dźwięk
        AudioManager.Play(opening ? "Menu_In" : "Menu_Out");

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
        isVisible = visible;

        isFading = false;
    }
}
