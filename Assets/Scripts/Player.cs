using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 16f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private float horizontalInput;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput < 0)
            sprite.flipX = true;
        else if (horizontalInput > 0)
            sprite.flipX = false;

        if (horizontalInput != 0 && isGrounded)
        {
            animator.ResetTrigger("End");
            animator.SetTrigger("Run");
        }
        else if (horizontalInput == 0 && isGrounded)
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("End");
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;

            animator.ResetTrigger("End");
            animator.ResetTrigger("Run");
            animator.SetTrigger("Jump");
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (!isGrounded)
        {
            animator.ResetTrigger("Run");
            animator.ResetTrigger("End");
            animator.SetTrigger("Jump");
        }
        if (isGrounded)
        {
            animator.SetTrigger("JumpEnd");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
