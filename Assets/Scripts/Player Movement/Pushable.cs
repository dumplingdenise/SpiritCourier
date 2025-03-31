using UnityEngine;

public class Pushable : MonoBehaviour
{
    private Vector2 lastValidPosition;
    public LayerMask groundLayer;

    void Start()
    {
        lastValidPosition = transform.position;
    }

    void Update()
    {
        // Store the last valid position before moving
        lastValidPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
           // Debug.Log("Obstacle hit a wall!");
            transform.position = lastValidPosition; // Prevent movement through walls
        }
    }
}