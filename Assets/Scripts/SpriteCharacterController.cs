using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCharacterController : MonoBehaviour
{
    [Header("Character Properties")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float attackCd = 0.1f;

    Rigidbody2D rb;
    Animator animator;
    Timer attackCdTimer;

    Vector2 m_movement;
    Vector2 m_lastMovement;
    bool m_isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackCdTimer = new Timer(attackCd);
    }

    public void SetMovement(Vector2 input)
    {
        m_movement.x = input.x;
        m_movement.y = input.y;
    }

    public void PerformAttack()
    {
        if (!m_isAttacking)
        {
            m_isAttacking = true;
            attackCdTimer.StartTimer();
            animator.SetTrigger("Attack");
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
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + m_movement * moveSpeed * Time.fixedDeltaTime);
    }
}