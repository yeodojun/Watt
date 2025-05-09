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
    private bool isAttack1 = false;
    public GameObject bladeAttack1;
    public GameObject bladeAttack2;
    private Vector2[] originalPoints;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        var poly1 = bladeAttack1.GetComponent<PolygonCollider2D>();
        if (poly1 != null)
        {
            originalPoints = poly1.GetPath(0);
        }
        var poly2 = bladeAttack2.GetComponent<PolygonCollider2D>();
        if (poly2 != null)
        {
            originalPoints = poly2.GetPath(0);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -1f;
        else
            horizontalInput = 0f;

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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isAttack1 == false)
            {
                animator.SetTrigger("Attack1");
                ActivateBladeAttack1();
                isAttack1 = true;
            }
            else if (isAttack1 == true)
            {
                animator.SetTrigger("Attack2");
                ActivateBladeAttack2();
                isAttack1 = false;
            }
        }
    }
    void ActivateBladeAttack1()
    {
        bladeAttack1.SetActive(true);
        var bladeRenderer = bladeAttack1.GetComponent<SpriteRenderer>();
        if (bladeRenderer != null)
            bladeRenderer.flipX = sprite.flipX;

        var poly1 = bladeAttack1.GetComponent<PolygonCollider2D>();
        if (poly1 != null && originalPoints != null)
        {
            Vector2[] flippedPoints = new Vector2[originalPoints.Length];
            for (int i = 0; i < originalPoints.Length; i++)
            {
                Vector2 p = originalPoints[i];
                p.x *= sprite.flipX ? -1 : 1;
                flippedPoints[i] = p;
            }
            poly1.SetPath(0, flippedPoints);
        }

        Animator bladeAnimator1 = bladeAttack1.GetComponent<Animator>();
        if (bladeAnimator1 != null)
        {
            bladeAnimator1.Play("BladeAttack1", 0, 0f);
        }
    }
    void ActivateBladeAttack2()
    {
        bladeAttack2.SetActive(true);
        var bladeRenderer = bladeAttack2.GetComponent<SpriteRenderer>();
        if (bladeRenderer != null)
            bladeRenderer.flipX = sprite.flipX;

        var poly2 = bladeAttack2.GetComponent<PolygonCollider2D>();
        if (poly2 != null && originalPoints != null)
        {
            Vector2[] flippedPoints = new Vector2[originalPoints.Length];
            for (int i = 0; i < originalPoints.Length; i++)
            {
                Vector2 p = originalPoints[i];
                p.x *= sprite.flipX ? -1 : 1;
                flippedPoints[i] = p;
            }
            poly2.SetPath(0, flippedPoints);
        }

        Animator bladeAnimator2 = bladeAttack2.GetComponent<Animator>();
        if (bladeAnimator2 != null)
        {
            bladeAnimator2.Play("BladeAttack2", 0, 0f);
        }
    }
    public void OnAttackEnd()
    {
        animator.SetTrigger("AttackEnd");
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
