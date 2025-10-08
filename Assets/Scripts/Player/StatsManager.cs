using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public StatsUI statsUI;

    [Header("Combat Stats")]
    public int damage;
    public float weaponRange;
    public float knockbackForce;
    public float kockbackTime;
    public float stunTime;

    [Header("Movement Stats")]
    public float moveSpeed;
    public float sprintSpeed;
    public float walkingFootstepSpeed = 0.4f;
    public float sprintingFootstepSpeed = 0.3f;
    public float footstepTimer = 0f;


    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
        statsUI.UpdateAllStats();
    }
    public void UpdateCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        statsUI.UpdateAllStats();
    }
    public void UpdateSpeed(float amount)
    {
        moveSpeed += amount;
        statsUI.UpdateAllStats();
    }
    public void UpdateSprintSpeed(float amount)
    {
        sprintSpeed += amount;
        statsUI.UpdateAllStats();
    }
    public void UpdateDamage(int amount)
    {
        damage += amount;
        statsUI.UpdateAllStats();
    }
}
