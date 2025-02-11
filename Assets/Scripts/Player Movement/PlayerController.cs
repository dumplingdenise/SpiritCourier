using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool MoveWhenTalking = true;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    // animator = GetComponent<Animator>();
    // private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (MoveWhenTalking)
            rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
       
    }

    public void SetMoveWhenTalking(bool value)
    {
        MoveWhenTalking = value;
        if (!MoveWhenTalking)
        {
            rb.linearVelocity = moveInput;
        }
    }

    private void OnMove()
    {
        if (!MoveWhenTalking) return;
        if (rb.linearVelocity != moveInput)
        {
            Vector2 targetPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

        }
    }


    public void HandleUpdate()
    {
        if (MoveWhenTalking && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Interact();
        }
        

    }

    void Interact()
    {
        // if (animator.runtimeAnimatorController == null)
        // {
        //     Debug.LogWarning("AnimatorController is missing or not assigned!");
        //     return;
        // }

        // Vector3 facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
        Vector3 facingDir = Vector3.up;  // Default direction for interaction if no animator
        Vector3 interactPos = transform.position + facingDir;

        // Debug.DrawLine(transform.position, interactPos, Color.red, 1f);

        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }


}

/* 
   public float moveSpeed = 5f;

    // private bool isMoving;

    private Vector2 moveInput;
    private Rigidbody2D rb;
   // private Animator animator;
    public bool CanMove = true; //movement control flag

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       // animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (CanMove)
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void SetCanMove(bool value)
    {
        CanMove = value;
        if (!CanMove)
        {
            moveInput = Vector2.zero;  // Stop movement immediately
           // animator.SetBool("isMoving", false);
        }
    }

    private void Move()
    {
        if (!CanMove) return;
        if (moveInput != Vector2.zero)
        {
            Vector2 targetPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            // animation parameters
            // animator.SetFloat("MoveX", moveInput.x);
            //animator.SetFloat("MoveY", moveInput.y);
            //animator.SetBool("isMoving", true);
        }
      //  else
        //{
            // animator.SetBool("isMoving", false);
       // }
    }





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
*/
