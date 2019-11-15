using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkBehavior : MonoBehaviour
{
    public float movementForce = 5;
    public float maxHorizontalVelocity = 2;

    Rigidbody2D body;
    SpriteRenderer sprite;

    enum Direction {left, right};
    Direction enemyDir;

    // Start is called before the first frame update
    void Start(){

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        enemyDir = Direction.left;
    }

    // Update is called once per frame
    void Update(){

        //flip direction if stuck on wall and flip the sprite
        if(body.velocity.x == 0.0) {
            if (enemyDir == Direction.left) {
                enemyDir = Direction.right;
                sprite.flipX = !sprite.flipX;
            }
                
            else if (enemyDir == Direction.right) {
                enemyDir = Direction.left;
                sprite.flipX = !sprite.flipX;
            }
                
        }

        //apply movement force
        if(enemyDir == Direction.left){
            body.AddForce(new Vector2(-1, 0) * movementForce);
        }
        else{
            body.AddForce(new Vector2(1, 0) * movementForce);
        }

        //restrict top speed
        if(body.velocity.x > maxHorizontalVelocity) {
            body.velocity = new Vector2(maxHorizontalVelocity, body.velocity.y);
        }
        if (body.velocity.x < -maxHorizontalVelocity) {
            body.velocity = new Vector2(-maxHorizontalVelocity, body.velocity.y);
        }
    }
}
