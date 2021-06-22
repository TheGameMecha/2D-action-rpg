﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding2D))]
[RequireComponent(typeof(SpriteCharacterController))]
public class EnemyAIController : MonoBehaviour
{
    SpriteCharacterController controller;
    Pathfinding2D pathfinder;

    int currentPathNode = 0;

    private void Start()
    {
        pathfinder = GetComponent<Pathfinding2D>();
        controller = GetComponent<SpriteCharacterController>();
        FindPathToPlayer();
    }

    public void FindPathToPlayer()
    {
        pathfinder.FindPath(transform.position, GameManager.instance.player.transform.position);
    }

    private void FixedUpdate()
    {
        if (controller.GetIsStunned() == false && !GameManager.instance.isUiActive)
        {
            FindPathToPlayer();
            if (pathfinder.path != null)
            {
                if (currentPathNode >= pathfinder.path.Count)
                {
                    currentPathNode = 0;
                    pathfinder.path = null;
                    //controller.StopAllMovement();
                }
                else if (Vector2.Distance(transform.position, pathfinder.path[currentPathNode].worldPosition) > 0.1f)
                {
                    Vector2 dir = pathfinder.path[currentPathNode].worldPosition - transform.position;
                    controller.SetMovement(dir.normalized);
                }
                else if (Vector2.Distance(transform.position, pathfinder.path[currentPathNode].worldPosition) < 0.1f)
                {
                    currentPathNode++;
                }
            }
        }
    }
}
