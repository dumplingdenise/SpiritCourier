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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.linearVelocity = direction * moveSpeed; // Fixed the velocity issue

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
        if (isInteracting) return;

        Debug.Log("Player interacted with moving NPC.");
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        isInteracting = true;
        isMoving = false;
        rb.linearVelocity = Vector2.zero; // Stop NPC movement
        rb.isKinematic = true; // Prevent NPC from pushing the player

        yield return DialogManager.Instance.ShowDialog(npcDialog);

        isInteracting = false;
        isMoving = true;
        rb.isKinematic = false; // Re-enable movement
    }
}
