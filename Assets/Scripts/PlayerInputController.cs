using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All player input code goes here
/// </summary>
[RequireComponent(typeof(SpriteCharacterController))]
public class PlayerInputController : MonoBehaviour
{
    SpriteCharacterController controller;
    Vector2 input;

    private void Awake()
    {
        controller = GetComponent<SpriteCharacterController>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        controller.SetMovement(input);

        if (Input.GetButtonDown("Fire1"))
        {
            controller.PerformAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            controller.PerformInteraction();
        }
    }
}
