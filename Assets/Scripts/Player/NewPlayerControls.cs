using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerControls : MonoBehaviour {
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode keyJump;
    public KeyCode keyInteract;

    public float maxVelocityX;
    public float maxVelocityY;

    public float jumpPower;

    public float bonusGravity;

    private Rigidbody2D rb;

    private bool inAir;
    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        inAir = true;
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        Debug.Log(inAir.ToString());
        Debug.Log(Input.GetAxisRaw("Horizontal").ToString());
        ProcessPhysics();
        ProcessInput();
        AdjustVelocity();
    }

    /////////////////////////////
    //  Main Helper Functions  //
    /////////////////////////////
    private void Jump() {
        Vector2 vel = rb.velocity;
        vel.y += jumpPower;
        rb.velocity = vel;
    }

    private void Move() {
        Vector2 vel = rb.velocity;
        vel.x += Input.GetAxisRaw("Horizontal") * maxVelocityY;
        rb.velocity = vel;
    }

    private void Interact() {

    }

    /////////////////////////////
    //    Process Functions    //
    /////////////////////////////
    private void ProcessPhysics() {
        if (inAir) {
            Vector2 vel = rb.velocity;
            vel.y -= bonusGravity * Time.deltaTime;
            rb.velocity = vel;
        }

        AdjustVelocity();
    }

    private void ProcessInput() {
        if (Input.GetAxisRaw("Horizontal") != 0) Move();
        if (Input.GetKeyDown(keyJump)) Jump();
    }

    // Keep the velocity of the player within the set margins
    private void AdjustVelocity() {
        if (rb.velocity.x > maxVelocityX) {
            rb.velocity = new Vector2(maxVelocityX, rb.velocity.y);
        }
        if (rb.velocity.x < -maxVelocityX) {
            rb.velocity = new Vector2(-maxVelocityX, rb.velocity.y);
        }
        if (rb.velocity.y > maxVelocityY) {
            rb.velocity = new Vector2(rb.velocity.x, maxVelocityY);
        }
        if (rb.velocity.y < -maxVelocityY) {
            rb.velocity = new Vector2(rb.velocity.x, -maxVelocityY);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ENV_GROUND")) {
            inAir = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("ENV_GROUND")) {
            inAir = true;
        }
    }
}
