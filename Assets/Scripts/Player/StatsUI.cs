using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;

    private bool statsOpen = false;

    private void Start()
    {
        UpdateAllStats();
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))
        {
            if (statsOpen)
            {
                Time.timeScale = 1;
                UpdateAllStats();
                statsCanvas.alpha = 0;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                UpdateAllStats();
                statsCanvas.alpha = 1;
                statsOpen = true;
            }
        }
    }

    public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.damage;
    }
    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.moveSpeed;
    }
    public void UpdateMaxHealth()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Max Health: " + StatsManager.Instance.maxHealth;
    }
    public void UpdateCurrentHealth()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Current Health: " + StatsManager.Instance.currentHealth;
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateMaxHealth();
        UpdateCurrentHealth();
    }
}
