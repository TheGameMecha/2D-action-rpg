using System.Collections;
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
    BoxCollider2D hitBox;
    Animator animator;
    Timer attackCdTimer;
    Timer knockbackTimer;

    Vector2 m_movement;
    Vector2 m_lastMovement;
    bool m_isAttacking;
    bool m_isStunned;
    FacingDirection m_facingDirection = FacingDirection.SOUTH;
    Vector2 m_facingVector;

    Collider2D m_interactableInRange;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();

        attackCdTimer = new Timer(attackCooldown);
        knockbackTimer = new Timer(0.2f);

        attackCdTimer.onTimerCompleted += AttackCDComplete;
        knockbackTimer.onTimerCompleted += KnockbackComplete;
    }

    #region Callback Functions
    void KnockbackComplete()
    {
        m_isStunned = false;
        StopAllMovement();
    }

    void AttackCDComplete()
    {
        m_isAttacking = false;
    }
    #endregion

    public void SetMovement(Vector2 input)
    {
        if (!m_isAttacking && !m_isStunned)
        {
            m_movement.x = input.x;
            m_movement.y = input.y;
        }
    }

    public void PerformKnockback(Vector2 direction, float force)
    {
        m_isStunned = true;
        m_movement = direction * force;

        knockbackTimer.StartTimer();
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

            if (hitTargets[i].GetComponent<Entity2D>())
            {
                hitTargets[i].GetComponent<Entity2D>().OnDamage(this);
            }
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

    protected override void Update()
    {
        base.Update();

        if (m_movement.sqrMagnitude > 0 && !m_isStunned) // !m_isStunned is used here to keep the facing direction during a knockback
        {
            m_lastMovement = m_movement;
        }

        // Update Animations
        animator.SetFloat("Horizontal", m_lastMovement.x); // Use lastMovement so we can keep the idle animations facing the correct direction
        animator.SetFloat("Vertical", m_lastMovement.y);
        animator.SetFloat("Speed", m_movement.sqrMagnitude);

        UpdateFacingDirection();
        CheckForInteract();

        attackCdTimer.Tick(Time.deltaTime);
        knockbackTimer.Tick(Time.deltaTime);

        if (m_isAttacking)
            HandleAttackDetection();
        
    }

    void FixedUpdate()
    {
        if (!m_isAttacking)
        {
            Vector2 currentPosition = rb.position;

            Vector2 deltaPosition = m_movement * movementSpeed;
            float final_hor_vel = deltaPosition.x;
            float final_ver_vel = deltaPosition.y;

            //horizontal check
            if (ScanWorldForCollision(currentPosition.x + m_movement.x * Time.fixedDeltaTime, currentPosition.y, GameManager.instance.WorldCollisionMask))
            {
                while (!ScanWorldForCollision(currentPosition.x + ((final_hor_vel * Time.fixedDeltaTime) / 8), currentPosition.y, GameManager.instance.WorldCollisionMask))
                {
                    currentPosition.x += ((final_hor_vel * Time.fixedDeltaTime) / 8);
                }
                final_hor_vel = 0;
            }
            currentPosition.x += final_hor_vel * Time.fixedDeltaTime;

            //vertical check
            if (ScanWorldForCollision(currentPosition.x, currentPosition.y + final_ver_vel * Time.fixedDeltaTime, GameManager.instance.WorldCollisionMask))
            {
                while (!ScanWorldForCollision(currentPosition.x, currentPosition.y + ((final_ver_vel * Time.fixedDeltaTime) / 8), GameManager.instance.WorldCollisionMask))
                {
                    currentPosition.y += ((final_ver_vel * Time.fixedDeltaTime) / 8);
                }
                final_ver_vel = 0;
            }
            currentPosition.y += final_ver_vel * Time.fixedDeltaTime;

            rb.MovePosition(currentPosition);
        }
    }

    public void StopAllMovement()
    {
        m_movement = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && m_isAttacking)
            Gizmos.DrawWireSphere(rb.position + (m_facingVector / 2), attackRange);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (ScanWorldForCollision(collision.transform.position.x, collision.transform.position.y, combatLayers))
        {
            Debug.Log("Collision With Enemy!");
            if (collision.gameObject.GetComponent<Entity2D>().entityType == EntityType.ENEMY)
            {
                Debug.Log("Player Was Hit");
                PerformKnockback(collision.GetContact(0).normal, 2.0f);
            }
        }
    }

    public override void OnDamage(Entity2D other)
    {
        base.OnDamage(other);
        PerformKnockback(other.GetComponent<SpriteCharacterController>().m_facingVector, 2.0f);
    }

    private bool ScanWorldForCollision(float xPos, float yPos, LayerMask mask)
    {
        Vector2 position = new Vector2(xPos + hitBox.offset.x, yPos + hitBox.offset.y);
        Collider2D[] collider2d = Physics2D.OverlapBoxAll(position, hitBox.size, 0, mask);
        return collider2d.Length > 0;
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