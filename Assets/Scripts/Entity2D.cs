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
    Collider2D col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
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