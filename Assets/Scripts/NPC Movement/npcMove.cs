using UnityEngine;
using System.Collections;

public class npcMove : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 minWalkPoints;
    private Vector2 maxWalkPoints;

    private Rigidbody2D myRigidbody;
    public bool isWalking;

    public float walkTime;
    private float walkCounter;

    public float waitTime;
    private float waitCounter;

    private int walkDirection; // Define walkDirection

    public Collider2D walkZone;
    private bool hasWalkZone;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        waitCounter = waitTime;
        walkCounter = walkTime;
               
        ChooseDirection();

        if(walkZone != null)
        {
            minWalkPoints = walkZone.bounds.min;
            maxWalkPoints = walkZone.bounds.max;
            hasWalkZone = true;
        }         
    }

    void Update()
    {   
        
        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }

            switch (walkDirection)
            {
                case 0:
                    myRigidbody.linearVelocity = new Vector2(0, moveSpeed);
                    if(hasWalkZone && transform.position.y > maxWalkPoints.y)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 1:
                    myRigidbody.linearVelocity = new Vector2(moveSpeed, 0);
                    if (hasWalkZone && transform.position.x > maxWalkPoints.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 2:
                    myRigidbody.linearVelocity = new Vector2(0, -moveSpeed);
                    if (hasWalkZone && transform.position.y < minWalkPoints.y)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;

                case 3:
                    myRigidbody.linearVelocity = new Vector2(-moveSpeed, 0);
                    if (hasWalkZone && transform.position.x < minWalkPoints.x)
                    {
                        isWalking = false;
                        waitCounter = waitTime;
                    }
                    break;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;
            myRigidbody.linearVelocity = Vector2.zero;

            if (waitCounter < 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }
}
