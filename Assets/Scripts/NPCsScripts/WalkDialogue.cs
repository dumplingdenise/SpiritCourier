using System.Collections;
using UnityEngine;

public class WalkDialogue : MonoBehaviour, Interactable
{
    public float moveSpeed = 2f;
    public Transform[] waypoints;  // Set waypoints for NPC movement
    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private Rigidbody2D rb;

    [SerializeField] Dialog npcDialog;
    private bool isInteracting = false;

    private Collider2D playerCollider;  // Reference to the player's collider
    private Collider2D npcCollider;     // Reference to NPC's collider

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        npcCollider = GetComponent<Collider2D>(); // Get NPC's collider
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(); // Assuming the player has the tag "Player"
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (!isMoving || isInteracting)
            return;

        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

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
            if (!isInteracting)
            {
                MoveToNextWaypoint();
            }
        }
    }

    public void Interact()
    {
        if (isInteracting) return;  // Prevent interaction if already interacting

        Debug.Log("Player interacted with moving NPC.");
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        if (isInteracting) yield break;  // Prevent re-entering if already interacting

        isInteracting = true;
        isMoving = false;
        rb.linearVelocity = Vector2.zero; // Stop NPC movement
        rb.bodyType = RigidbodyType2D.Kinematic; // Set NPC to Kinematic to stop physics

        // Disable collision to prevent pushing the player
        npcCollider.enabled = false;
        playerCollider.enabled = false;

        yield return DialogManager.Instance.ShowDialog(npcDialog);

        // After dialogue ends
        isInteracting = false;
        isMoving = true;
        rb.bodyType = RigidbodyType2D.Dynamic; // Set NPC back to dynamic for normal movement
        npcCollider.enabled = true;  // Re-enable NPC collision
        playerCollider.enabled = true; // Re-enable player collision
    }
}
