using System.Collections;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public float fadeOutDuration = 1.5f;

    [Header("Loot Drop")]
    public GameObject lootPrefab;
    public ItemSO lootItem;
    public int lootQuantity = 1;

    private bool isDead = false;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Enemy_Movement enemyMovement;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyMovement = GetComponent<Enemy_Movement>();
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return; // nie reaguj po śmierci

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {
        isDead = true;

        // zatrzymujemy ruch i atak
        if (enemyMovement != null)
        {
            enemyMovement.ChangeState(EnemyState.Idle);
            enemyMovement.enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb) rb.linearVelocity = Vector2.zero;
        }

        // uruchom animację śmierci
        if (anim != null)
        {
            anim.SetBool("isDead", true);
        }

        // czekaj, aż animacja się skończy
        yield return new WaitForSeconds(GetAnimationLength("Slime_Death"));

        // uruchom fade out
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        bool lootSpawned = false;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeOutDuration;
            float alpha = Mathf.Lerp(1f, 0f, progress);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // pojawienie się lootu
            if (!lootSpawned && alpha <= 0.5f)
            {
                SpawnLoot();
                lootSpawned = true;
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private float GetAnimationLength(string animName)
    {
        if (anim == null || anim.runtimeAnimatorController == null) return 0.5f;

        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
                return clip.length;
        }

        return 0.5f;
    }

    private void SpawnLoot()
    {
        if (lootPrefab != null && lootItem != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0f, -0.5f, 0f);
            GameObject lootObj = Instantiate(lootPrefab, spawnPosition, Quaternion.identity);

            Loot loot = lootObj.GetComponent<Loot>();
            if (loot != null)
            {
                loot.Initialize(lootItem, lootQuantity);
            }

            BounceEffect bounce = lootObj.GetComponent<BounceEffect>();
            if (bounce != null)
            {
                bounce.StartBounce();
            }
        }
    }
}
