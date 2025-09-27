using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public PlayerHealth playerHealth;

    private float lowHealthTimer = 0f;
    private float lowHealthInterval = 1.2f; // co ile sekund dźwięk

    void Update()
    {
        health = StatsManager.Instance.currentHealth;
        maxHealth = StatsManager.Instance.maxHealth;
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        // LowHealth SFX w pętli gdy zdrowie = 1
        if (health == 1)
        {
            lowHealthTimer -= Time.unscaledDeltaTime;
            if (lowHealthTimer <= 0f)
            {
                AudioManager.Play("LowHealth");
                lowHealthTimer = lowHealthInterval;
            }
        }
        else
        {
            lowHealthTimer = 0f;
        }
    }
}
