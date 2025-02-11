using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    public LayerMask solidObjectsLayer;

    public LayerMask interactablesLayer;

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos)) 
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool isWalkable(Vector3 targetPos) 
    {
        if (Physics2D.OverlapCircle(targetPos, 0.05f, solidObjectsLayer | interactablesLayer) != null) 
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black; // Change color for visibility

        // Draw a circle at the current player's position
        Gizmos.DrawWireSphere(transform.position, 1f);

        // Predict the next position based on input
        Vector2 nextPos = (Vector2)transform.position + input;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(nextPos, 0.05f); // Shows where collision is being checked
    }

}