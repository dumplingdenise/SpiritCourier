using UnityEngine;

public class Pushable : MonoBehaviour
{
    private Vector2 lastValidPosition;
    public LayerMask groundLayer;

    public GameObject interactionIndicator; // Assign in Inspector

    private bool canShowIndicator = true; // Controls visibility of the indicator

    void Start()
    {
        lastValidPosition = transform.position;
        interactionIndicator.SetActive(false);
    }

    void Update()
    {
        lastValidPosition = transform.position;
    }

    public void ShowIndicator(bool show)
    {
        if (canShowIndicator)
        {
            interactionIndicator.SetActive(show);
        }
    }

    public void DisableIndicator()
    {
        canShowIndicator = false;
        interactionIndicator.SetActive(false);
    }

    public void EnableIndicator()
    {
        canShowIndicator = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            transform.position = lastValidPosition;
            ShowIndicator(true);
        }
    }
}



/*using UnityEngine;

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
*/