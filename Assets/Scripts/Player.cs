using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode action;

    public float speed;
    public float jumpHeight;
    public int pickaxeLevel;

    public float moveBuffer; // How far the axis needs to be to start moving

    private Rigidbody2D rb;

    private float playerFacing;

    private bool grounded;
    private bool canWallJump;
    private int wallJumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = false;
        canWallJump = false;
        wallJumpCount = 1;
        playerFacing = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    void MoveCharacter() {
        if (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0) {
            UpdateFacing();
            if (rb.velocity.x <= speed) {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed,rb.velocity.y);
            }
        } else if (Input.GetAxisRaw("Horizontal") == 0) {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if (Input.GetKeyDown(jump) && grounded) {
            Jump();
        } else if (Input.GetKeyDown(jump) && canWallJump && wallJumpCount > 0) {
            WallJump();
        }
    }

    //***************************************
    // Movement functions
    //***************************************
    private void UpdateFacing() {
        if (Input.GetKey(left)) {
            playerFacing = -1f;
        } else if (Input.GetKey(right)) {
            playerFacing = 1f;
        }
    }
    private void Jump() {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpHeight),ForceMode2D.Impulse);
    }
    private void WallJump() {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerFacing * 0.75f, jumpHeight * 2f),ForceMode2D.Impulse);
        wallJumpCount--;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ENV_GROUND")) {
            grounded = true;
            wallJumpCount = 1;
        }

        if (collision.gameObject.CompareTag("ENV_WALL")) {
            canWallJump = true;
        } else if (collision.gameObject.CompareTag("ENV_WALL_NOGRIP")) {
            // Do something?
        }

        if (collision.gameObject.CompareTag("ENV_BREAKABLE")) {
            if (Input.GetKeyDown(action)) {
                if (pickaxeLevel >= collision.gameObject.GetComponent<Wall_Breakable_Script>().GetRequiredPickaxeLevel()) {
                    collision.gameObject.GetComponent<Wall_Breakable_Script>().TakeDamage(1f);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ENV_GROUND")) {
            grounded = true;
            wallJumpCount = 1;
        }

        if (collision.gameObject.CompareTag("ENV_WALL")) {
            if (Input.GetKey(action)) {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
        }

        if (collision.gameObject.CompareTag("ENV_BREAKABLE")) {
            if (Input.GetKeyDown(action)) {
                if (pickaxeLevel >= collision.gameObject.GetComponent<Wall_Breakable_Script>().GetRequiredPickaxeLevel()) {
                    collision.gameObject.GetComponent<Wall_Breakable_Script>().TakeDamage(1f);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ENV_GROUND")) {
            grounded = false;
        } else if (collision.gameObject.CompareTag("ENV_WALL")) {
            canWallJump = false;
        }
    }
}
