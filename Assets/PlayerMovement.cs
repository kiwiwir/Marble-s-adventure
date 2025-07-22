using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    Animator anim;
    private Vector2 lastMoveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        ProccessInputs();
        Animate();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed; // poprawka!
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
    }
    void Animate()
    {
        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);
        anim.SetFloat("MoveMagnitude", moveInput.magnitude);
        anim.SetFloat("LastInputX", lastMoveDirection.x);
        anim.SetFloat("LastInputY", lastMoveDirection.y);
    }
}