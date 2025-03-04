using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform[] waypoints;  // Set waypoints for NPC movement
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private Rigidbody2D rb;

    private Animator animator;  // Reference to Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Get the Animator component
        StartCoroutine(Patrol());
    }

    void Update()
    {
        // Only move if isMoving is true
        if (!isMoving) return;

        // Call method to move NPC
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Set the velocity for movement
        rb.linearVelocity = direction * moveSpeed;

        // Set the animation parameters (same as player)
        animator.SetBool("isWalking", true);  // Start walking animation
        animator.SetFloat("InputX", direction.x);  // Set horizontal movement direction
        animator.SetFloat("InputY", direction.y);  // Set vertical movement direction

        // Check if the NPC has reached the current waypoint
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!isMoving)
            {
                // Stop movement and reset animation if not moving
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("isWalking", false);  // Stop walking animation
            }
            else
            {
                // Continue moving to the next waypoint
                MoveToNextWaypoint();
            }
        }
    }
}
















/*using UnityEngine;
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
/*  }

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
*/