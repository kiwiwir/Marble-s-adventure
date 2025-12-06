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

    private void Start()
    {
        FindSpawnPoint();
    }
    private void FindSpawnPoint()
    {
        if (spawnPoint == null)
        {
            var obj = GameObject.Find("SpawnPoint");
            if (obj != null)
                spawnPoint = obj.transform;
            else
                Debug.LogWarning("SpawnPoint not found!");
        }
    }

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
        
        if (spawnPoint == null || spawnPoint.Equals(null))
        {
            Debug.Log("SpawnPoint reference was missing — trying to find it again.");
            FindSpawnPoint();
        }

        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint STILL missing — cannot respawn player!");
            return;
        }

        if (player != null)
        {
            player.SetActive(true);

            // ustawienie order on layer na 2
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 2;
            }

            // zresetuj HP
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StatsManager.Instance.currentHealth = playerFullHealth;
                playerHealth.isDead = false;
            }

            // zresetuj stan ruchu / koloru
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ResetState();
            }

            // przesuń na spawn point
            player.transform.position = spawnPoint.position;

            Debug.Log("Player respawned at " + spawnPoint.position);
        }
    }
}
