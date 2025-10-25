using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    private bool isSprinting = false;
    private bool isKnockedBack;

    Animator anim;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material knockbackMaterial; // przypisz SolidGreenMaterial w Inspectorze
    private Material defaultMaterial;

    public Player_Combat player_Combat;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }

    void Update()
    {
        ProccessInputs();
        Animate();
        HandleFootsteps();

        if (Input.GetButtonDown("Attack"))
        {
            player_Combat.Attack();
        }
    }
    private void FixedUpdate()
    {
        if (isKnockedBack == false)
        {
            float speed = isSprinting ? StatsManager.Instance.sprintSpeed : StatsManager.Instance.moveSpeed;
            rb.linearVelocity = moveInput * speed;
        }
    }

    void ProccessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 currentInput = new Vector2(moveX, moveY);

        if (currentInput.magnitude > 0.1f)
        {
            lastMoveDirection = currentInput.normalized;
        }

        moveInput = currentInput.normalized;

        isSprinting = Keyboard.current != null && Keyboard.current.shiftKey.isPressed;
    }
    void Animate()
    {
        bool isWalking = moveInput.magnitude > 0.1f;

        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);
        anim.SetFloat("LastInputX", lastMoveDirection.x);
        anim.SetFloat("LastInputY", lastMoveDirection.y);
        anim.SetBool("isSprinting", isSprinting);
        anim.SetBool("isWalking", isWalking);
    }

    void HandleFootsteps()
    {
        if (moveInput.magnitude > 0.1f)
        {
            StatsManager.Instance.footstepTimer -= Time.deltaTime;

            if (StatsManager.Instance.footstepTimer <= 0f)
            {
                PlayFootsteps();
                StatsManager.Instance.footstepTimer = isSprinting ? StatsManager.Instance.sprintingFootstepSpeed : StatsManager.Instance.walkingFootstepSpeed;
            }
        }
        else
        {
            StatsManager.Instance.footstepTimer = 0f; // reset timeru, jeśli nie chodzimy
        }
    }

    public void PlayFootsteps()
    {
        AudioManager.Play("Footsteps", true);
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));

        // podmień materiał na jednolity kolor
        spriteRenderer.material = knockbackMaterial;
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;

        // przywróć oryginalny materiał
        spriteRenderer.material = defaultMaterial;
    }

    public void ResetState()
    {
        StopAllCoroutines(); // zatrzymuje np. KnockbackCounter
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
        spriteRenderer.material = defaultMaterial;
    }
}