using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMovement(InputValue value) {
        movement = value.Get<Vector2>();

        if (movement.x != 0 || movement.y != 0) {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("IsWalking", true);
        } else {
            animator.SetBool("IsWalking", false);
        }
    }

    private void FixedUpdate() {
        // Variant 1
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        
        // Variant 2
        // if (movement.x != 0 || movement.y != 0) {
        // rb.velocity = movement * speed;
        // }

        // Variant 3
        // rb.AddForce(movement * speed);

        // Variant 4
        // MovePlayer();
    }

    // private void MovePlayer() {
    //     if (movement.magnitude != 0) {
    //         Vector2 moveInput = movement.normalized * speed * Time.fixedDeltaTime;
    //         rb.MovePosition(rb.position + moveInput);
    //     }
    // }
}
