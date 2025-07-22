using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float sprintSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    Animator anim;
    private Vector2 lastMoveDirection;
    private bool isSprinting = false;

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
        float speed = isSprinting ? sprintSpeed : moveSpeed;
        rb.linearVelocity = moveInput * speed;
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
}