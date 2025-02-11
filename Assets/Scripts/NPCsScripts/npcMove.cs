using UnityEngine;
using System.Collections;

public class npcMove : MonoBehaviour
{
    public float moveSpeed; //Speed of NPCs
    private Vector2 minWalkPoints; //boundaries of the walkzone
    private Vector2 maxWalkPoints; //boundaries of the walkzone

    private Rigidbody2D myRigidbody;
    public bool isWalking;

    public float walkTime; //How long Npcs move before stopping
    private float walkCounter; //timer for movement/waiting

    public float waitTime; //how long NPC wait
    private float waitCounter; //timer for movement/waiting

    private int walkDirection; // Define walkDirection / raondomly chose movement direction

    // public Collider2D walkZone; //define Npc movement area
   // private bool hasWalkZone; //check if walk zone is set

    public LayerMask collisionLayers; // Assign GroundLayer and Overpass in Inspector


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();

       /* if (walkZone != null)
        {
            minWalkPoints = walkZone.bounds.min;
            maxWalkPoints = walkZone.bounds.max;
            hasWalkZone = true;
        }
        */
    }

    void Update()
    {
        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

            Vector2 movement = Vector2.zero;

            switch (walkDirection)
            {
                case 0:
                    movement = Vector2.up * moveSpeed;
                    break;
                case 1: 
                    movement = Vector2.right * moveSpeed;
                    break;
                case 2: 
                    movement = Vector2.down * moveSpeed;
                    break;
                case 3: 
                    movement = Vector2.left * moveSpeed; 
                    break;
            }

            // Check for collisions before moving
            if (!IsColliding(movement))
            {
                myRigidbody.linearVelocity = movement;
            }
            else
            {
                isWalking = false; // Stop movement if colliding
                waitCounter = waitTime;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;
            myRigidbody.linearVelocity = Vector2.zero;

            if (waitCounter < 0)
            {
                ChooseDirection();
            }
        }

    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }

    bool IsColliding(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.2f, collisionLayers);
        return hit.collider != null; // Returns true if there's a collision
    }

}
