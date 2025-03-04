using UnityEngine;
using UnityEngine.UI;

public class MiniDot : MonoBehaviour
{
    public Transform player; // Drag your player here in the Inspector
    public RectTransform minimapPanel; // Drag your minimap UI (Raw Image)
    public float minimapSize = 50f; // Adjust this based on your minimap size

    void Update()
    {
        if (player == null || minimapPanel == null) return;

        // Get player's position in world space
        Vector3 playerPos = player.position;

        // Normalize position to fit within minimap
        float xPos = (playerPos.x / minimapSize) * minimapPanel.rect.width;
        float yPos = (playerPos.y / minimapSize) * minimapPanel.rect.height;

        // Update the PlayerDot position relative to the Minimap
        GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
    }
}
