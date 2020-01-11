using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : PhysicsCust
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetFloat("SpeedY", 0);
        animator.SetBool("Jumped", false);
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

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            animator.SetBool("Jumped", true);
        }
        // Allows jump cancellation
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * .5f;
            }
        }

        if (!Input.GetButtonDown("Jump") && grounded)
        {
            animator.SetBool("Jumped", false);
        }

        // Ensures Sprite is Facing Correct Direction
        if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }


        animator.SetBool("Grounded", grounded);
        // animator.SetFloat("velocityX", Mathf.Abs(velocity.x)/maxSpeed);
        animator.SetFloat("SpeedY", Mathf.Abs(velocity.y)/maxSpeed);
        targetVelocity = move * maxSpeed;
    }
}
