using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    public LayerMask interactableLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {       
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            Debug.Log("This is input.x" + input.x);
            Debug.Log("This is input.y" + input.y);

            if (input != Vector2.zero)
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }
        }

       // animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.F))
            Interact();
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
        var interactPos = transform.position + facingDir;

        // Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();  //calling the interact function of the interactable interface (i think)
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
}