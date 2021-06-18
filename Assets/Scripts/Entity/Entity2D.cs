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

    Timer damageImmunityTimer;

    Collider2D col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        damageImmunityTimer = new Timer(damageImmunityTime);
    }

    protected virtual void Update()
    {
        damageImmunityTimer.Tick(Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public virtual void OnDamage(Entity2D other)
    {
        if (!damageImmunityTimer.IsDone())
            return;

        damageImmunityTimer.StartTimer();
        Debug.Log(gameObject.name + " Damaged by: " + other.name);
    }

    public virtual void OnDamage(Entity2D other, int damage = 0)
    {
        OnDamage(other);
    }

    public virtual void OnDamage(Entity2D other, float damage = 0.0f)
    {
        OnDamage(other);
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