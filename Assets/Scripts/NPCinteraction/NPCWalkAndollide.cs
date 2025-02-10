using UnityEngine;

public class NPCWalkAndollide : MonoBehaviour
{

    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private float moveTimer;
    public float moveDuration = 2f;
    public float waitDuration = 1f;
    private bool isMoving = false;

    public LayerMask collisionLayers; // Assign GroundLayer and Overpass in Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickRandomDirection();
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                isMoving = false;
                moveTimer = waitDuration;
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                PickRandomDirection();
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 newPosition = (Vector2)transform.position + movementDirection * moveSpeed * Time.fixedDeltaTime;

            // Check if the NPC will collide with anything
            RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection, 0.5f, collisionLayers);
            if (hit.collider == null)
            {
                rb.MovePosition(newPosition);
            }
            else
            {
                isMoving = false; // Stop movement if colliding
                moveTimer = waitDuration;
            }
        }
    }

    void PickRandomDirection()
    {
        int randomDir = Random.Range(0, 4);
        switch (randomDir)
        {
            case 0: movementDirection = Vector2.up; break;
            case 1: movementDirection = Vector2.right; break;
            case 2: movementDirection = Vector2.down; break;
            case 3: movementDirection = Vector2.left; break;
        }

        isMoving = true;
        moveTimer = moveDuration;
    }
}