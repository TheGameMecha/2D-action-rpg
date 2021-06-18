using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Entity2D is any GameObject that exists within the world that can involve interaction beyond just collision
/// This includes players, NPCs, enemies, traps, chests, doors, etc.
/// This is the base class for all other Entities to derive from
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Entity2D : MonoBehaviour
{
    public EntityType entityType = EntityType.OBJECT;
    public float damageImmunityTime = 1.0f;

    protected bool m_isDamageImmune;

    Timer damageImmunityTimer;

    Collider2D col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        damageImmunityTimer = new Timer(damageImmunityTime);
        damageImmunityTimer.onTimerCompleted += EndDamageImmunity;
    }

    protected virtual void Update()
    {
        damageImmunityTimer.Tick(Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void EndDamageImmunity()
    {
        m_isDamageImmune = false;
    }

    public virtual void OnDamage(Entity2D other, int damage = 0)
    {
        if (m_isDamageImmune)
            return;

        m_isDamageImmune = true;
        damageImmunityTimer.StartTimer();
        Debug.Log(gameObject.name + " Damaged by: " + other.name);
    }

    public virtual void Teleport(Vector2 position)
    {
        transform.position = position;
    }
}

[System.Flags]
public enum EntityType
{
    PLAYER,
    ENEMY,
    OBJECT,
    NPC,
    INTERACTABLE
}