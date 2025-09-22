using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float sprintSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    Animator anim;
    private Vector2 lastMoveDirection;
    private bool isSprinting = false;

    private bool isKnockedBack;

    [SerializeField] private float walkingFootstepSpeed = 0.4f;
    [SerializeField] private float sprintingFootstepSpeed = 0.3f;
    [SerializeField] private float footstepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ProccessInputs();
        Animate();
        HandleFootsteps();
    }
    private void FixedUpdate()
    {
        if (isKnockedBack == false)
        {
            float speed = isSprinting ? sprintSpeed : moveSpeed;
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
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootsteps();
                footstepTimer = isSprinting ? sprintingFootstepSpeed : walkingFootstepSpeed;
            }
        }
        else
        {
            footstepTimer = 0f; // reset timeru, jeśli nie chodzimy
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
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}