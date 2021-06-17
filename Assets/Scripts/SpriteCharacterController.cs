﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpriteCharacterController : Entity2D
{
    [Header("Character Properties")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float attackCooldown = 0.1f;
    [SerializeField] [Tooltip("The layers that this character can attack")] LayerMask combatLayers;
    [SerializeField] float attackRange = 1.0f;
    [SerializeField] float interactRange = 1.0f;

    Rigidbody2D rb;
    Animator animator;
    Timer attackCdTimer;

    Vector2 m_movement;
    Vector2 m_lastMovement;
    bool m_isAttacking;
    FacingDirection m_facingDirection = FacingDirection.SOUTH;
    Vector2 m_facingVector;

    Collider2D m_interactableInRange;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackCdTimer = new Timer(attackCooldown);
    }

    public void SetMovement(Vector2 input)
    {
        if (!m_isAttacking)
        {
            m_movement.x = input.x;
            m_movement.y = input.y;
        }
    }

    public void PerformAttack()
    {
        if (!m_isAttacking && m_interactableInRange == null)
        {
            m_isAttacking = true;
            attackCdTimer.StartTimer();
            animator.SetTrigger("Attack");
        }
    }

    public void PerformInteraction()
    {
        if (m_isAttacking)
            return;

        if (m_interactableInRange != null)
        {
            Debug.Log("Interacted with " + m_interactableInRange.name);

            if (m_interactableInRange.GetComponent<InteractableEntity2D>() != null)
            {
                m_interactableInRange.GetComponent<InteractableEntity2D>().InteractedWith(this);
            }
        }
    }

    void CheckForInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, m_facingVector, interactRange, GameManager.instance.interactableLayer);
        m_interactableInRange = hit.collider;
        Debug.DrawRay(rb.position, m_facingVector * interactRange);
    }

    void HandleAttackDetection()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(rb.position + (m_facingVector / 2), attackRange, combatLayers.value);

        for (int i = 0; i < hitTargets.Length; i++)
        {
            Debug.Log(hitTargets[i]);
        }
    }

    void UpdateFacingDirection()
    {
        if (m_lastMovement.x > 0 && m_lastMovement.y > 0)
        {
            m_facingDirection = FacingDirection.NORTH;
        }
        else if (m_lastMovement.x < 0 && m_lastMovement.y < 0)
        {
            m_facingDirection = FacingDirection.SOUTH;
        }
        else if (m_lastMovement.x < 0 && m_lastMovement.y > 0)
        {
            m_facingDirection = FacingDirection.NORTH;
        }
        else if (m_lastMovement.x > 0 && m_lastMovement.y < 0)
        {
            m_facingDirection = FacingDirection.SOUTH;
        }
        else if (m_lastMovement.x > 0)
        {
            m_facingDirection = FacingDirection.EAST;
        }
        else if (m_lastMovement.x < 0)
        {
            m_facingDirection = FacingDirection.WEST;
        }
        else if (m_lastMovement.y > 0)
        {
            m_facingDirection = FacingDirection.NORTH;
        }
        else if (m_lastMovement.y < 0)
        {
            m_facingDirection = FacingDirection.SOUTH;
        }


        m_facingVector = Vector2.right;

        switch (m_facingDirection)
        {
            case FacingDirection.NORTH:
                m_facingVector = Vector2.up;
                break;
            case FacingDirection.SOUTH:
                m_facingVector = -Vector2.up;
                break;
            case FacingDirection.EAST:
                m_facingVector = Vector2.right;
                break;
            case FacingDirection.WEST:
                m_facingVector = -Vector2.right;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (m_movement.sqrMagnitude > 0)
        {
            m_lastMovement = m_movement;
        }

        // Update Animations
        animator.SetFloat("Horizontal", m_lastMovement.x); // Use lastMovement so we can keep the idle animations facing the  correct direction
        animator.SetFloat("Vertical", m_lastMovement.y);
        animator.SetFloat("Speed", m_movement.sqrMagnitude);

        attackCdTimer.Tick(Time.deltaTime);

        if (attackCdTimer.IsDone())
        {
            m_isAttacking = false;
        }

        UpdateFacingDirection();
        CheckForInteract();

        if (m_isAttacking)
        {
            HandleAttackDetection();
        }
    }

    void FixedUpdate()
    {
        if (!m_isAttacking)
            rb.MovePosition(rb.position + m_movement * movementSpeed * Time.fixedDeltaTime);
    }

    public void StopAllMovement()
    {
        m_movement = Vector2.zero;
        //m_lastMovement = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && m_isAttacking)
            Gizmos.DrawWireSphere(rb.position + (m_facingVector / 2), attackRange);
    }
}

[System.Serializable]
public enum FacingDirection
{
    NORTH,
    SOUTH,
    EAST,
    WEST
}