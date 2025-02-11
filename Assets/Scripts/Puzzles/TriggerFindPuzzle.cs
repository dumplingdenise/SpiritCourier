using UnityEngine; // Only this is needed

public class TriggerFindPuzzle : MonoBehaviour
{
    [SerializeField] private FindingPuzzle waldoPuzzle; // Reference to the FindingPuzzle script
    [SerializeField] private string interactionKey = "e"; // The key to interact with the NPC (default is "E")

    private bool isPlayerInRange = false; // Flag to check if the player is near the NPC

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey)) // Player presses the interaction key when near the NPC
        {
            waldoPuzzle.StartPuzzle(); // Trigger the "Where's Waldo?" puzzle
        }
    }

    // Detect when the player is near the NPC (for interaction)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // When the player leaves the interaction range
        {
            isPlayerInRange = false;
        }
    }
}
