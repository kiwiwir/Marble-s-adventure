using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RetryScript : MonoBehaviour
{
    public Transform spawnPoint;        // miejsce respawnu gracza poza wrogami
    public CanvasGroup gameOverUI;      // panel Game Over do fade-out
    public int playerFullHealth = 8;   // pełne HP po respawnie
    public float fadeDuration = 1f;     // czas fade-out
    public GameObject player;

    public void OnRetryClicked()
    {
        if (gameOverUI != null)
        {
            StartCoroutine(FadeOutAndRespawn());
        }
        else
        {
            RespawnPlayer();
        }
    }

    private IEnumerator FadeOutAndRespawn()
    {
        float elapsed = 0f;
        float startAlpha = gameOverUI.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            gameOverUI.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        gameOverUI.alpha = 0f;
        gameOverUI.gameObject.SetActive(false);

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Debug.Log("RespawnPlayer called");
        // znajdź gracza
        if (player != null)
        {
            player.transform.position = spawnPoint.position;
            player.SetActive(true);
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StatsManager.Instance.currentHealth = playerFullHealth;
                playerHealth.isDead = false;
            }
        }
    }
}
