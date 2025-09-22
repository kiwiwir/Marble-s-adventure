using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

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
}
