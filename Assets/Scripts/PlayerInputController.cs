using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// All player input code goes here
/// </summary>
[RequireComponent(typeof(SpriteCharacterController))]
public class PlayerInputController : MonoBehaviour
{
    SpriteCharacterController controller;
    Vector2 input;

    // Input Bools
    bool attack;
    bool interact;

    PlayerControlScheme controls;

    private void Awake()
    {
        controller = GetComponent<SpriteCharacterController>();
        controls = new PlayerControlScheme();

        controls.Player.Move.performed += ctx => input = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => input = Vector2.zero;
        controls.Player.Fire.performed += ctx => attack = true;
        controls.Player.Fire.canceled += ctx => attack = false;
        controls.Player.Interact.performed += ctx => interact = true;
        controls.Player.Interact.canceled += ctx => interact = false;
    }

    void Update()
    {
        if (GameManager.instance.isUiActive)
        {
            controller.StopAllMovement();
            controls.Player.Disable();
            return;
        }
        else
        {
            controls.Player.Enable();
        }

        controller.SetMovement(input);

        if (attack)
        {
            controller.PerformAttack();
        }

        if (interact && GameManager.instance.isUiActive == false)
        {
            controller.PerformInteraction();
        }
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }
    void OnDisable()
    {
        controls.Player.Disable();
    }
}
