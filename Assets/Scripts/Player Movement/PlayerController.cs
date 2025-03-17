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

        // Toggle pushing mode when Q is pressed
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("Pushing mode toggled: " + isPushing);  // Debug log to check if pushing mode is activated
            isPushing = !isPushing;

            // Stop pushing immediately when Q is pressed
            if (!isPushing && pushableRb != null)
            {
                pushableRb.linearVelocity = Vector2.zero; // Stop movement immediately when push mode is off
                pushableRb = null;
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

      void FixedUpdate()
    {
        // Only push the object when Q is pressed and a pushable object is in front
        if (isPushing)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 0.6f, interactableLayer);

            if (hit.collider != null && hit.collider.CompareTag("Pushable"))
            {
                pushableRb = hit.collider.GetComponent<Rigidbody2D>();
                if (pushableRb != null)
                {
                    pushableRb.linearVelocity = moveInput * pushForce; // Apply force only while moving
                    Debug.Log("Pushing object: " + hit.collider.name);  // Debug log to check if the object is detected
                }
            }
            else
            {
                pushableRb = null; // No object in front, stop pushing
            }
        }
        else
        {
            // Stop the object movement when not pushing
            if (pushableRb != null)
            {
                pushableRb.linearVelocity = Vector2.zero; // Stop the object
                pushableRb = null;
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

