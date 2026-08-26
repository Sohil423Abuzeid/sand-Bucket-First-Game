using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController2 : MonoBehaviour
{
    public PlayerMovementStats stats;
    public BoxCollider2D feetCollider;
    public BoxCollider2D headCollider;
    public CircleCollider2D circleCollider;

    private Rigidbody2D rb;
    private Animator animator;
    private cameraController cameraController;
    // Movement & Velocity
    [HideInInspector]
    public Vector2 moveInput;
    [HideInInspector]
    public Vector2 currentVelocity;
    private bool isRunning;
    [HideInInspector]
    public bool isFacingRight = true;

    // Jump calculations based on GDC formulas
    private float gravity;
    private float initialJumpVelocity;

    // State & Timers
    private bool isGrounded;
    private bool bumpedHead;
    private float jumpBufferTimer;
    private float coyoteTimer;
    private int jumpsUsed;

    private bool isJumping;
    private bool isFastFalling;
    private bool pastApexThreshold;
    private float timePastApex;
    private float timeFastFalling;

    // Dash Variables
    [HideInInspector]
    public bool isDashing; 
    private bool isAirDashing;
    private bool isDashFastFalling;
    private bool wasAirDashing;
    private int dashesUsed;
    private Vector2 dashDirection;

    // Timers
    private float dashTimer;
    private float dashOnGroundTimer;
    private float dashFastFallTime;
    private float dashFastFallReleaseSpeed;

    //Spawn 
    private Vector2 spawn;
    private void Awake()
    {
        spawn = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cameraController = GameObject.FindAnyObjectByType<cameraController>();
        circleCollider = gameObject.GetComponentsInChildren<CircleCollider2D>()[0];
        Debug.Log(circleCollider);
        // Math formulas mentioned in the video to calculate precise jump arcs
        gravity = -(2 * stats.jumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
        initialJumpVelocity = Mathf.Abs(gravity) * stats.timeToJumpApex;
    }

    private void Update()
    {
        GatherInput();
        UpdateTimers();
        CheckJumpInput();
        DashCheck();
    }

    private void FixedUpdate()
    {
        HandleAnimation();
        CheckCollisions();
        HandleDash();
        HandleMovement();
        HandleJumpPhysics();
        ApplyVelocity();
    }

    private void HandleAnimation()
    {
        animator.SetBool("moving_bool", moveInput.x != 0);
        animator.SetFloat("velocity_float",isGrounded?0: currentVelocity.y);
        animator.SetBool("dash_bool", isDashing);
        animator.SetBool("wasAirDashing_bool", wasAirDashing);
    }
    private void GatherInput()
    {
        // Replace with your exact New Input System references
        // e.g., moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    private void UpdateTimers()
    {
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
    }

    private void CheckCollisions()
    {
        // Ground Check via BoxCast from feet
        isGrounded = Physics2D.BoxCast(feetCollider.bounds.center, feetCollider.bounds.size, 0f, Vector2.down, stats.groundDetectionRayLength, stats.groundLayer);

        // Head Check via BoxCast from head
        if (headCollider != null)
        {
            bumpedHead = Physics2D.BoxCast(headCollider.bounds.center, headCollider.bounds.size, 0f, Vector2.up, stats.headDetectionRayLength, stats.groundLayer);
        }

        if (isGrounded)
        {
            coyoteTimer = stats.coyoteTime;
            jumpsUsed = 0;
            isJumping = false;
            isFastFalling = false;
            
            if(wasAirDashing)
            {
                dashesUsed = Mathf.Max(0, dashesUsed-1);
                wasAirDashing = false;
            }
        }
        else if (coyoteTimer > 0 && currentVelocity.y < 0)
        {
            // If we fall off a ledge without jumping, consume a jump so we don't get a free double jump
            coyoteTimer = 0;
            jumpsUsed++;
        }
    }

    private void HandleMovement()
    {
        if (isDashing) return;
        // Turn the player
        if (moveInput.x != 0)
        {
            if (moveInput.x > 0 && !isFacingRight) Turn();
            if (moveInput.x < 0 && isFacingRight) Turn();
        }

        // Calculate horizontal acceleration/deceleration based on ground state
        float targetSpeed = moveInput.x * (isRunning ? stats.runSpeed : stats.walkSpeed);
        float accelRate = isGrounded ? stats.groundAcceleration : stats.airAcceleration;
        float decelRate = isGrounded ? stats.groundDeceleration : stats.airDeceleration;

        if (moveInput.x != 0)
        {
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, 0f, decelRate * Time.fixedDeltaTime);
        }
    }

    private void CheckJumpInput()
    {
        // Jump Pressed
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jumpBufferTimer = stats.jumpBufferTime;
        }

        // Jump Released (Triggers Jump Cut / Fast Fall)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isJumping && currentVelocity.y > 0)
            {
                isFastFalling = true;
            }
        }

        // Initiate Jump if buffer is active and we meet conditions
        if (jumpBufferTimer > 0)
        {
            if ((isGrounded || coyoteTimer > 0) || (jumpsUsed < stats.allowedJumps))
            {
                animator.SetTrigger("jump_trigger");
                InitiateJump();
            }
        }
    }

    private void InitiateJump()
    {
        isJumping = true;
        isFastFalling = false;
        jumpBufferTimer = 0;
        coyoteTimer = 0;

        jumpsUsed++;

        currentVelocity.y = initialJumpVelocity * stats.jumpCutCompensationFactor;
    }

    private void HandleJumpPhysics()
    {
        if (isDashing) return;

        if (bumpedHead)
        {
            isFastFalling = true;
        }

        if (isJumping)
        {
            // Apex Hang Time Check
            if (Mathf.Abs(currentVelocity.y) < stats.apexThreshold)
            {
                pastApexThreshold = true;
                timePastApex += Time.fixedDeltaTime;

                if (timePastApex < stats.apexHangTime)
                {
                    currentVelocity.y = 0f; // Float at the apex briefly
                }
                else
                {
                    currentVelocity.y += gravity * Time.fixedDeltaTime;
                }
            }
            else
            {
                pastApexThreshold = false;
                timePastApex = 0f;

                // Normal Ascending/Descending Gravity
                if (isFastFalling && currentVelocity.y > 0)
                {
                    // Jump Cut mechanics (falling faster because jump was released)
                    currentVelocity.y += gravity * stats.gravityMultiplierOnRelease * Time.fixedDeltaTime;
                }
                else
                {
                    currentVelocity.y += gravity * Time.fixedDeltaTime;
                }
            }
        }
        else if (!isGrounded)
        {
            // Normal Gravity when simply falling
            currentVelocity.y += gravity * Time.fixedDeltaTime;
        }

        // Clamp to Max Fall Speed
        currentVelocity.y = Mathf.Clamp(currentVelocity.y, -stats.maxFallSpeed, float.MaxValue);
    }

    private void ApplyVelocity()
    {
        rb.velocity = currentVelocity;
    }

    private void Turn()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    // under this all dash (fun)s
    private void DashCheck()
    {
        if (isDashing) return;

        bool dashPressed = Input.GetKeyDown(KeyCode.LeftShift); // Replace with your input

        if (dashPressed)
        {
            // Ground Dash Check
            if (  isGrounded && !isDashing&& dashesUsed < stats.allowedDashes)
            {
                InitiateDash();
            }
            // Air Dash Check
            else if (!isGrounded && !isDashing && dashesUsed < stats.allowedDashes)
            {
                isAirDashing = true;
                InitiateDash();
            }
        }
    }
    private void InitiateDash()
    {
        dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        dashesUsed++;
        isDashing = true;
        dashTimer = 0f;

        isJumping = false;
    }
    private void HandleDash()
    {
        if (isDashing)
        {
            dashTimer += Time.fixedDeltaTime;

            if (dashTimer >= stats.dashTime)
            {
                // End Dash
                isDashing = false;
                isAirDashing = false;

                if (!isGrounded)
                {
                    isDashFastFalling = true;
                    wasAirDashing = true;
                    dashFastFallTime = 0f;
                    dashFastFallReleaseSpeed = currentVelocity.y;
                }
                else
                {
                    StartCoroutine(ResetDashCounter());
                }
                return;
            }

            // Lock velocity to dash speed
            currentVelocity.x = dashDirection.x * stats.dashSpeed;
            if (dashDirection.y != 0 || isAirDashing)
            {
                currentVelocity.y = dashDirection.y * stats.dashSpeed;
            }
        }
        else if (isDashFastFalling)
        {
            // Smoothly cancel upward momentum after an upward dash finishes
            if (currentVelocity.y > 0)
            {
                dashFastFallTime += Time.fixedDeltaTime;
                if (dashFastFallTime < stats.timeForUpwardsCancel)
                {
                    currentVelocity.y = Mathf.Lerp(dashFastFallReleaseSpeed, 0f, dashFastFallTime / stats.timeForUpwardsCancel);
                }
                else
                {
                    currentVelocity.y += gravity * stats.dashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }
            }
            else
            {
                isDashFastFalling = false;
            }
        }
    }
    private IEnumerator ResetDashCounter()
    {
        yield return new WaitForSeconds(stats.dashCooldown);

        dashesUsed = Mathf.Max(0, dashesUsed - 1);
    }

    // spawn and void trigger 
    public void resetSpawn(Vector2 vector2)
    {
        spawn = vector2;
    }

    private void respawn()
    {
        transform.position = spawn;
        cameraController.ResetPostion();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.voidlimit.ToString()))
        {
            respawn();
        }
    }


}
