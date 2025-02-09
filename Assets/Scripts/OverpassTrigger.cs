using UnityEngine;

public class OverpassTrigger : MonoBehaviour
{
    private bool isWalkingOverpass = false;
    private SpriteRenderer[] defaultLayerSpriteRenderers; // Declare sprite renderers

    void Start()
    {
        // Initialize sprite renderers (get all renderers in the player object)
        defaultLayerSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {

    }

    void UpdateSortingAndCollisionLayer()
    {
        if (isWalkingOverpass)
        {
            SetSortingLayer("WalkingRoof");
        }
        else
        {
            SetSortingLayer("Player");
        }
    }
        void SetSortingLayer(string layername)
        {
            foreach (SpriteRenderer spriteRenderer in defaultLayerSpriteRenderers)
            {
                spriteRenderer.sortingLayerName = layername;
            }
        }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        Debug.Log("Triggered: " + collider2D.gameObject.name);

        if (collider2D.CompareTag("Underpass"))
        {
            isWalkingOverpass = false;
            Debug.Log("Walking Underpass");
            UpdateSortingAndCollisionLayer();
        }
        else if (collider2D.CompareTag("Overpass"))
        {
            isWalkingOverpass = true;
            Debug.Log("Walking Overpass");
            UpdateSortingAndCollisionLayer();
        }
    }



}

