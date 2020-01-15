using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerControls : PhysicsCust
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float wallJumpBoost = 12;

    public KeyCode action;

    public float pickAxeRange;
    public LayerMask climbableWallLayer;
    
    // Right == true
    // Left  == false
    private bool facing = true;

    // True if wall jumping
    private bool boosting = false;

    // Int equals number of wall jumps player has left
    private int canWallJump = 1;

    // Testing needs to be changed to deltatime
    private int wallJumpTimer = 30;
    private bool wallJumpAnimationOn = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetFloat("SpeedY", 0);
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
        UpdatePlayerAction();
    }

    protected void UpdatePlayerAction()
    {

    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (velocity.y < 0.1f && grounded)
        {
            boosting = false;
            wallJumpAnimationOn = false;
            canWallJump = 1;
        }

        // Checks if player should wall jump
        if (Input.GetKeyDown(action))
        {
            if (CanWallJump() && (canWallJump > 0))
            {
                WallJump();
            }
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            animator.SetBool("Jumped", true);
            Debug.Log("jump");
        }

        // Allows jump cancellation
        else if (Input.GetButtonUp("Jump"))
        {   
            // When boosting(walljumping)jump cancellation is stopped
            if (velocity.y > 0 && !boosting)
            {
                velocity.y = velocity.y * .5f;
                Debug.Log("jumpcancel");
            }
        }

        UpdateFacing();

        AnimatorUpdates();

        targetVelocity = move * maxSpeed;
    }

    private void UpdateFacing()
    {
        // Ensures Sprite is Facing Correct Direction
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            // Player is facing left
            facing = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            // Player is facing right
            facing = true;
        }
    }

    private void AnimatorUpdates()
    {
        animator.SetFloat("VelocityX", Math.Abs(velocity.x));
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("SpeedY", Mathf.Abs(velocity.y) / maxSpeed);

        if (velocity.y < 0.1f && grounded)
        {
            animator.SetBool("Jumped", false);
            animator.SetBool("WallJumped", false);
        }

        if (wallJumpAnimationOn)
        {
            wallJumpTimer--;
        }

        if (wallJumpTimer == 0)
        {
            animator.SetBool("WallJumped", false);
            wallJumpAnimationOn = false;
            wallJumpTimer = 30;
        }
    }

    private void WallJump()
    {
        float velocityFromJump;

        // Limit Transferable Velocity From Jump 
        if (Math.Abs(velocity.y) > (jumpTakeOffSpeed / 4))
        {
            velocityFromJump = (jumpTakeOffSpeed / 4);
        }
        else
        {
            velocityFromJump = Math.Abs(velocity.y);
        }

        velocity.y = velocityFromJump + wallJumpBoost * maxSpeed;
        boosting = true;
        wallJumpAnimationOn = true;
        canWallJump--;
        Debug.Log("boost");
    }

    private bool CanWallJump()
    {

        Vector2 direction;

        // Check which direction player is facing
        if (facing)
        {
            direction = Vector2.right;
            Debug.Log("right");
        }
        else
        {
            direction = Vector2.left;
            Debug.Log("left");
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, pickAxeRange, climbableWallLayer);

        if (hit.collider != null)
        {
            Debug.Log("hitwall");
            animator.SetBool("WallJumped", true);
            return true;
        }

        return false;
    }
}
