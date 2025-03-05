using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool MoveWhenTalking = true; // flag to control movement while talking

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
 
    private Animator animator;
    public Animator playerAnimator; //player animation


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        //only allow movement if MoveWhenTalking is true
        if (MoveWhenTalking)
            rb.linearVelocity = moveInput * moveSpeed;

    }


    public void Move(InputAction.CallbackContext context)
    {
        if (!MoveWhenTalking) return;  // Prevent movement when talking

        moveInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            rb.linearVelocity = Vector2.zero;  // Stop movement
        }
        else
        {
            animator.SetBool("isWalking", true);
            rb.linearVelocity = moveInput * moveSpeed; // Apply movement
        }

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        // test code end
    }

    public void SetMoveWhenTalking(bool value)
    {
        MoveWhenTalking = value;

        if (!MoveWhenTalking)
        {
            // test code
            moveInput = Vector2.zero; // reset input to prevent movement
            // end test code

            rb.linearVelocity = Vector2.zero;  // Ensure player stops
            animator.SetBool("isWalking", false);
        }
    }

    // look like this is not used.
/*    private void OnMove()
    {
        if (!MoveWhenTalking) return;

        if (rb.linearVelocity != moveInput)
        {
            Vector2 targetPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

        }
    }*/


    public void HandleUpdate()
    {
        if (MoveWhenTalking && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("F key pressed for interaction.");
            Interact();
        }
        

    }

    void Interact()
    {

        Vector3 facingDir = Vector3.up;  // Default direction for interaction if no animator
        Vector3 interactPos = transform.position + facingDir;


        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }

    }
}

