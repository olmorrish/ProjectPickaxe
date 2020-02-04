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
    public float pickRange;

    public float moveBuffer; // How far the axis needs to be to start moving

    private Rigidbody2D rb;
    private TriggerScript ts;

    public float playerFacing;

    private bool grounded;
    private bool canWallJump;
    private int wallJumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        ts = gameObject.GetComponent<TriggerScript>();
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

        if (Input.GetKeyDown(jump) && inRange("ENV_WALL", pickRange) && wallJumpCount > 0)
        {
            WallJump();
        } else if (Input.GetKeyDown(jump) && grounded) {
            Jump();
        }

    }

    //***************************************
    // Movement functions
    //***************************************
    private void UpdateFacing() {
        if (Input.GetAxisRaw("Horizontal") < 0f) {

            playerFacing = -1f;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponentsInChildren<BoxCollider2D>()[1].offset = new Vector2(-0.25f, 0.15f);
        } else if (Input.GetAxisRaw("Horizontal") > 0f) {
            playerFacing = 1f;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponentsInChildren<BoxCollider2D>()[1].offset = new Vector2(0.25f, 0.15f);
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

        /*
        if (collision.gameObject.CompareTag("ENV_WALL")) {
            if (Input.GetKey(action)) {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
        }
        */

    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ENV_GROUND")) {
            grounded = false;
        } 
        /*
        else if (collision.gameObject.CompareTag("ENV_WALL")) {
            canWallJump = false;
        }
        */
    }

    private bool inRange(string tag, float minDistance)
    {
        GameObject[] objTag = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < objTag.Length; ++i)
        {
            if (Vector3.Distance(transform.position, objTag[i].transform.position) <= Mathf.Abs(minDistance))
                return true;
        }

        return false;
    }
}


