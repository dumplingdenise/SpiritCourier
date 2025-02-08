using UnityEngine;

public class RoofTransition : MonoBehaviour
{
    public SpriteRenderer playerRenderer;
    public string onRoofLayer = "HouseRoof";  // Player above roof
    public string belowRoofLayer = "Player";  // Default player layer

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRenderer.sortingLayerName = onRoofLayer;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRenderer.sortingLayerName = belowRoofLayer;
        }
    }
}

