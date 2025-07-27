using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC_Wander : MonoBehaviour
{
    [Header("Wander Area")]
    public float wanderWidth = 5;
    public float wanderHeight = 5;
    public Vector2 startingPosition;
    public float pauseDuration = 1;
    public float speed = 2;
    public Vector2 target;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isPaused;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
        StartCoroutine(PauseAndPickNewDestination());
    }
    private void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(PauseAndPickNewDestination());
        }

        Move();
    }

    private void Move()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        rb.linearVelocity = direction * speed;
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
        anim.Play("Walk");
    }
    public void OollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(PauseAndPickNewDestination());
    }
    private Vector2 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2;
        float halfHeight = wanderHeight / 2;
        int edge = Random.Range(0, 3);

        return edge switch
        {
            0 => new Vector2(startingPosition.x - halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)), // Left edge
            1 => new Vector2(startingPosition.x + halfWidth, Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)), // Right edge
            2 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y - halfHeight), // Bottom edge
            3 => new Vector2(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), startingPosition.y + halfHeight), // Top edge
        };
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, wanderHeight, 0));
    }
}
