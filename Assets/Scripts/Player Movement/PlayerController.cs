using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


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

    private bool playingFootsteps = false;
    public float footsepSpeed = 0.5f;
    private AudioSource audioSource; // Add this line

    private bool isPushing = false;
    private Rigidbody2D pushableRb;
    public float pushForce = 5f; // Adjust for push strength

    private Vector2 lastMoveDirection = Vector2.down; // Default facing down
    private float pushCheckDistance = 0.6f; //distance to check if still near the box


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        // Get the AudioSource attached to the player
        audioSource = GetComponent<AudioSource>(); // Add this line

    }

    void Update()
    {
        if (MoveWhenTalking)
            rb.linearVelocity = moveInput * moveSpeed;

        if (rb.linearVelocity.magnitude > 0 && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if (rb.linearVelocity.magnitude == 0 && playingFootsteps)
        {
            StopFootsteps();
        }

        // Update last move direction only when moving
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }

        // Handle pushing toggle
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (isPushing)
            {
                StopPushing();
            }
            else
            {
                TryStartPushing();
            }
        }
    }

   
    /* void Update()
     {
         //only allow movement if MoveWhenTalking is true
         if (MoveWhenTalking)
             rb.linearVelocity = moveInput * moveSpeed;

         if (rb.linearVelocity.magnitude  > 0 && !playingFootsteps)
         {
             StartFootsteps();
         }
         else if(rb.linearVelocity.magnitude == 0)
         {
             StopFootsteps();
         }

     }*/

    public void Move(InputAction.CallbackContext context)
    {
        if (!MoveWhenTalking)  // Prevent movement during dialogue
        {
            moveInput = Vector2.zero; // Reset input
            rb.linearVelocity = Vector2.zero; // Stop movement
            animator.SetBool("isWalking", false);
            StopFootsteps();
            return;
        }

        moveInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            rb.linearVelocity = Vector2.zero;  // Stop movement
            StopFootsteps();
        }
        else
        {
            animator.SetBool("isWalking", true);
            rb.linearVelocity = moveInput * moveSpeed; // Apply movement
            StartFootsteps();
        }

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

    }


    public void SetMoveWhenTalking(bool value)
    {
        MoveWhenTalking = value;

        if (!MoveWhenTalking)
        {
            moveInput = Vector2.zero; // Clear movement input
            rb.linearVelocity = Vector2.zero;  // Stop movement
            animator.SetBool("isWalking", false);
            StopFootsteps();
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
    void TryStartPushing()
    {
        if (lastMoveDirection == Vector2.zero) return;  // Player must be facing a direction

        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastMoveDirection, pushCheckDistance, interactableLayer);

        if (hit.collider != null && hit.collider.CompareTag("Pushable"))
        {
            pushableRb = hit.collider.GetComponent<Rigidbody2D>();
            if (pushableRb != null)
            {
                pushableRb.bodyType = RigidbodyType2D.Dynamic;
                isPushing = true;
                Debug.Log("Pushing started.");
            }
        }
        else
        {
            Debug.Log("No pushable object in front.");
        }
    }

    void StopPushing()
    {
        if (pushableRb != null)
        {
            pushableRb.linearVelocity = Vector2.zero;
            pushableRb.bodyType = RigidbodyType2D.Kinematic;
            pushableRb = null;
        }
        isPushing = false;
        Debug.Log("Stopped pushing.");
    }

    void FixedUpdate()
    {
        if (isPushing && pushableRb != null)
        {
            // Check if the player is still in front of the box
            RaycastHit2D checkHit = Physics2D.Raycast(transform.position, lastMoveDirection, pushCheckDistance, interactableLayer);

            if (checkHit.collider == null || checkHit.collider.gameObject != pushableRb.gameObject)
            {
                StopPushing(); // Stop pushing if the player is no longer near the box
                return;
            }

           // Apply push only in the last move direction
            if (moveInput == lastMoveDirection)
            {
                pushableRb.linearVelocity = lastMoveDirection * pushForce;
            }
            else
            {
                pushableRb.linearVelocity = Vector2.zero; // Stop box movement if player moves in another direction
            }
        }
    }
    void StartFootsteps()
        {
            if (!playingFootsteps)
            {
                playingFootsteps = true;
                SoundEffectManager.Play("Footstep");
            }
        }

        void StopFootsteps()
        {
            if (playingFootsteps)
            {
                playingFootsteps = false;
                SoundEffectManager.Stop();  // Stop the sound completely
            }
        }

        void PlayFootstep()
        {
            if (playingFootsteps && !audioSource.isPlaying) // Prevent overlapping sounds
            {
                Debug.Log("Footstep sound playing");
                SoundEffectManager.Play("Footstep");
            }
        }
}

