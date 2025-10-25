using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public CanvasGroup gameOverUI; // przypisz panel GameOver w inspectorze
    public float fadeDuration = 1.5f; // czas fade-in
    public bool isDead = false;

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;

        if (StatsManager.Instance.currentHealth <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        // Poczekaj jedną klatkę aby zaktualizować HealthUI
        yield return null;

        AudioManager.Play("GameOver");
        isDead = true;

        // Włącz panel GameOver
        if (gameOverUI != null)
        {
            gameOverUI.gameObject.SetActive(true);
            gameOverUI.alpha = 0f;
            gameOverUI.interactable = false;
            gameOverUI.blocksRaycasts = false;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                gameOverUI.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            gameOverUI.alpha = 1f;
            gameOverUI.interactable = true;
            gameOverUI.blocksRaycasts = true;
        }

        gameObject.SetActive(false);
    }
}
