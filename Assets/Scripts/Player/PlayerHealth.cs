using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
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

        gameObject.SetActive(false);
    }
}
