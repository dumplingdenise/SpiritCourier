using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Audio;


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

