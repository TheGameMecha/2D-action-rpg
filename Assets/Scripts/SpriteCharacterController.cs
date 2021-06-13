using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCharacterController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D rb;
    Animator animator;

    Vector2 movement;
    Vector2 lastMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetMovement(Vector2 input)
    {
        movement.x = input.x;
        movement.y = input.y;
    }

    void Update()
    {
        if (movement.sqrMagnitude > 0)
        {
            lastMovement = movement;
        }

        // Update Animations
        animator.SetFloat("Horizontal", lastMovement.x); // Use lastMovement so we can keep the idle animations facing the  correct direction
        animator.SetFloat("Vertical", lastMovement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}