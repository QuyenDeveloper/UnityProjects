using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEntity : MonoBehaviour
{
    private Rigidbody2D rb;
    private FighterStats fighterStats;
    private Vector2 movementDirection, oldMovementDirection;
    public Animator animator;
    public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fighterStats = GetComponent<FighterStats>();
    }
    private void FixedUpdate()
    {
        if(movementDirection.magnitude > 0 && fighterStats.CurrentSpeed >= 0)
        {
            oldMovementDirection = movementDirection;
            fighterStats.CurrentSpeed += fighterStats.Acceleration * fighterStats.MaxSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else
        {
            fighterStats.CurrentSpeed -= fighterStats.Deacceleration * fighterStats.MaxSpeed * Time.deltaTime;
            animator.SetBool("isWalking", false);
        }
        fighterStats.CurrentSpeed = Mathf.Clamp(fighterStats.CurrentSpeed, 0, fighterStats.MaxSpeed);
        rb.velocity = oldMovementDirection * fighterStats.CurrentSpeed;
    }
}
