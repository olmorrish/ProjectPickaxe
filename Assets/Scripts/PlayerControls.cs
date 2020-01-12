using System.Collections;
using System.Collections.Generic;
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
            return true;
        }

        return false;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (velocity.y < 0.1f && grounded)
        {
            animator.SetBool("Jumped", false);
            boosting = false;
        }

        // Checks if player should wall jump
        if (Input.GetKeyDown(action))
        {
            if (CanWallJump())
            {
                //velocity.y = 0;
                /*
                if (velocity.y > 0.1){
                    velocity.y = 1;
                }
                */

                velocity.y = wallJumpBoost * maxSpeed;
                boosting = true;
                Debug.Log("boost");
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


        animator.SetBool("Grounded", grounded);
        // animator.SetFloat("velocityX", Mathf.Abs(velocity.x)/maxSpeed);
        animator.SetFloat("SpeedY", Mathf.Abs(velocity.y)/maxSpeed);
       // animator.SetFloat("SpeedY", velocity.y);
        targetVelocity = move * maxSpeed;
    }
}
