using System;
using UnityEditor.Rendering.LookDev;
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
    private bool wasFalling = false;
    private bool isUp = false;
    private bool isAttack1 = false;
    public GameObject bladeAttack1;
    public GameObject bladeAttack2;
    public GameObject upBladeAttack;
    public GameObject downBladeAttack;
    private Vector2[] originalPoints1;
    private Vector2[] originalPoints2;
    private Vector2[] originalPoints3;
    private Vector2[] originalPoints4;

    public LayerMask groundLayer;
    public Vector2 groundCheckSize = new Vector2(0.8f, 0.1f);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        var poly1 = bladeAttack1.GetComponent<PolygonCollider2D>();
        if (poly1 != null)
        {
            originalPoints1 = poly1.GetPath(0);
        }
        var poly2 = bladeAttack2.GetComponent<PolygonCollider2D>();
        if (poly2 != null)
        {
            originalPoints2 = poly2.GetPath(0);
        }
        var poly3 = upBladeAttack.GetComponent<PolygonCollider2D>();
        if (poly3 != null)
        {
            originalPoints3 = poly3.GetPath(0);
        }
        var poly4 = downBladeAttack.GetComponent<PolygonCollider2D>();
        if (poly4 != null)
        {
            originalPoints4 = poly4.GetPath(0);
        }
    }

    void Update()
    {
        animator.SetBool("isUp", isUp);
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

        if (isGrounded)
        {
            if (horizontalInput != 0)
            {
                animator.ResetTrigger("End");
                animator.SetTrigger("Run");
            }
            else
            {
                animator.ResetTrigger("Run");
                animator.SetTrigger("End");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
            isGrounded = false;
        }
        if (rb.linearVelocity.y > 0f)
        {
            wasFalling = false;
            isUp = true;
            animator.SetBool("isUp", true);
        }

        if (!isGrounded && !wasFalling && rb.linearVelocity.y < -0.1f)
        {
            isUp = false;
            animator.SetBool("isUp", false);
            animator.ResetTrigger("Jump");
            animator.SetTrigger("JumpDown");
            animator.ResetTrigger("JumpLanding");
            wasFalling = true;
        }
        if (isGrounded)
        {
            wasFalling = false;
            animator.ResetTrigger("JumpDown");
            animator.SetTrigger("JumpLanding");
            animator.SetTrigger("JumpEnd");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetTrigger("UpAttack");
                ActivateUpBladeAttack();
            }
            else if (!isGrounded && Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetTrigger("DownAttack");
                ActivateDownBladeAttack();
            }
            else
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
    }
    void ActivateBladeAttack1()
    {
        bladeAttack1.SetActive(true);
        var bladeRenderer = bladeAttack1.GetComponent<SpriteRenderer>();
        if (bladeRenderer != null)
            bladeRenderer.flipX = sprite.flipX;

        var poly1 = bladeAttack1.GetComponent<PolygonCollider2D>();
        if (poly1 != null && originalPoints1 != null)
        {
            Vector2[] flippedPoints = new Vector2[originalPoints1.Length];
            for (int i = 0; i < originalPoints1.Length; i++)
            {
                Vector2 p = originalPoints1[i];
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
        if (poly2 != null && originalPoints2 != null)
        {
            Vector2[] flippedPoints = new Vector2[originalPoints2.Length];
            for (int i = 0; i < originalPoints2.Length; i++)
            {
                Vector2 p = originalPoints2[i];
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

    void ActivateUpBladeAttack()
    {
        upBladeAttack.SetActive(true);
        var bladeRenderer = upBladeAttack.GetComponent<SpriteRenderer>();
        if (bladeRenderer != null)
            bladeRenderer.flipX = sprite.flipX;

        var poly3 = upBladeAttack.GetComponent<PolygonCollider2D>();
        if (poly3 != null && originalPoints3 != null)
        {
            Vector2[] flippedPoints = new Vector2[originalPoints3.Length];
            for (int i = 0; i < originalPoints3.Length; i++)
            {
                Vector2 p = originalPoints3[i];
                p.x *= sprite.flipX ? -1 : 1;
                flippedPoints[i] = p;
            }
            poly3.SetPath(0, flippedPoints);
        }

        Animator UpBladeAttack = upBladeAttack.GetComponent<Animator>();
        if (UpBladeAttack != null)
        {
            UpBladeAttack.Play("UpBladeAttack", 0, 0f);
        }
    }

    void ActivateDownBladeAttack() 
    {
        downBladeAttack.SetActive(true);
        var bladeRenderer = downBladeAttack.GetComponent<SpriteRenderer>();
        if (bladeRenderer != null)
            bladeRenderer.flipX = sprite.flipX;

        var poly4 = upBladeAttack.GetComponent<PolygonCollider2D>();
        if (poly4 != null && originalPoints3 != null)
        {
            Vector2[] flippedPoints = new Vector2[originalPoints3.Length];
            for (int i = 0; i < originalPoints3.Length; i++)
            {
                Vector2 p = originalPoints3[i];
                p.x *= sprite.flipX ? -1 : 1;
                flippedPoints[i] = p;
            }
            poly4.SetPath(0, flippedPoints);
        }

        Animator DownBladeAttack = downBladeAttack.GetComponent<Animator>();
        if (DownBladeAttack != null)
        {
            DownBladeAttack.Play("DownBladeAttack", 0, 0f);
        }
    }
    public void OnAttackEnd()
    {
        animator.SetTrigger("AttackEnd");
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;

        velocity.x = horizontalInput * speed;

        if (velocity.y < 0)
        {
            velocity.y += Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            velocity.y += Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        rb.linearVelocity = velocity;

        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        isGrounded = Physics2D.BoxCast(origin, groundCheckSize, 0f, Vector2.down, 0.05f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
    }
}
