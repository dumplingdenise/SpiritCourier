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
            Debug.Log("Toggling pushing mode: " + isPushing);

            if (isPushing)
            {
                // Stop the pushing and set to Kinematic when Q is pressed to stop
                if (pushableRb != null)
                {
                    pushableRb.linearVelocity = Vector2.zero; // Stop the movement
                    pushableRb.bodyType = RigidbodyType2D.Kinematic; // Make it Kinematic to stop movement
                    pushableRb = null; // Reset the reference
                }
            }
            else
            {
                // Start pushing mode only if we are close to a pushable object
                RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 0.6f, interactableLayer);

                if (hit.collider != null && hit.collider.CompareTag("Pushable"))
                {
                    pushableRb = hit.collider.GetComponent<Rigidbody2D>();
                    if (pushableRb != null)
                    {
                        pushableRb.bodyType = RigidbodyType2D.Dynamic; // Make it dynamic to allow pushing
                        Debug.Log("Pushable object found and ready to push!");
                    }
                }
            }

            // Toggle the pushing flag
            isPushing = !isPushing;
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
        if (isPushing && pushableRb != null)
        {
            // Apply the pushing force in the direction of moveInput
            pushableRb.linearVelocity = moveInput * pushForce; // Apply force in the direction the player is moving
        }
        else if (!isPushing && pushableRb != null)
        {
            // When pushing mode is off, stop movement immediately and reset Rigidbody2D
            pushableRb.linearVelocity = Vector2.zero;
            pushableRb.bodyType = RigidbodyType2D.Kinematic;
            pushableRb = null; // Optionally clear the reference if you want
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

