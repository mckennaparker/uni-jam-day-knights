using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Jump Assistance")]
    [SerializeField] private float jumpBufferTime = 0.12f;
    private float jumpBufferCounter;

    [Header("Ground Check Settings")]
    public bool grounded;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    [SerializeField] private float footstepInterval = 0.2f;
    private float footstepTimer;
    private bool wasGrounded;

    private Rigidbody2D rb;
    private float horizInput;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        horizInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
        

        if (jumpBufferCounter > 0f && IsGrounded()) // while pressing: jumping up
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            //_animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0) // falling down
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }


        FlipDirection();
        UpdateAnimation();
        HandleLandingSound();
        HandleFootsteps();
        wasGrounded = grounded;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizInput * moveSpeed, rb.linearVelocity.y);
        GroundDetector();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0f, groundLayer);
    }

    private void FlipDirection()
    {
        if (facingRight && horizInput < 0f || !facingRight && horizInput > 0f)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("XVelocity", Mathf.Abs(horizInput));
        animator.SetFloat("YVelocity", rb.linearVelocity.y);
        animator.SetBool("IsGrounded", IsGrounded());
    }

    // Detect whether the player is grounded and then parent player to moving platform if on one
    private void GroundDetector()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheckPoint.position, 
                                                         groundCheckSize, 
                                                         0f, 
                                                         groundLayer);
        if (colliders.Length > 0)
        {
            grounded = true;

            foreach (var collider in colliders)
            {
                if (collider.tag == "MovingPlatform")
                {
                    transform.parent = collider.transform;
                } else
                {
                    transform.parent = null;
                }
            }
        } else
        {
            grounded = false;
        }
    }

    // Visualize Ground Detector
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }

    private void HandleFootsteps()
    {
        bool isWalking = grounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        if (isWalking)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                AudioManager.Instance?.PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
    private void HandleLandingSound()
    {
        if (!wasGrounded && grounded)
        {
            AudioManager.Instance?.PlayFootstep();
        }
    }
}