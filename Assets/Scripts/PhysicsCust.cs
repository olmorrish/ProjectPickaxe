using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCust : MonoBehaviour
{ 

    public float gravityScale = 1f;
    public float minGroundNormalY = .65f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;

    protected Vector2 velocity;
    protected Rigidbody2D rb;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    // So we can't pass into another collider
    protected const float almostRadius = 0.01f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        // Use 2D collision settings for which layers to check collision against
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        // Gravity
        velocity += gravityScale * Physics2D.gravity * Time.deltaTime;

        velocity.x = targetVelocity.x;

        grounded = false;

        // Gravity
        Vector2 deltaPosition = velocity * Time.deltaTime;

        // Calcing vector along slope/ground
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        // Check to minimize collision detection
        if (distance > minMoveDistance)
        {   
            // Check if there is a collider in direction we are moving
            int count = rb.Cast(move, contactFilter, hitBuffer, distance + almostRadius);

            hitBufferList.Clear();
            // List of rays that hit
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            // Determine angle of ray to surface
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                // Only counts as ground if > minGroundNormalY
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                
                // Edge case where player hits roof and must maintain horizontal velocity
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - almostRadius;
                if (distance > modifiedDistance)
                {
                    distance = modifiedDistance;
                }


            }

        }

        rb.position += move.normalized * distance;
    }
}
