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
    PlayerControlScheme controls;

    private void Awake()
    {
        controller = GetComponent<SpriteCharacterController>();
        controls = new PlayerControlScheme();

        controls.Player.Move.performed += ctx => input = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => input = Vector2.zero;
    }

    void Update()
    {
        if (GameManager.instance.isUiActive)
        {
            controller.StopAllMovement();
            return;
        }


        
        //input.x = Input.GetAxisRaw("Horizontal");
        //input.y = Input.GetAxisRaw("Vertical");

        controller.SetMovement(input);

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    controller.PerformAttack();
        //}

        //if (Input.GetButtonDown("Fire2"))
        //{
        //    controller.PerformInteraction();
        //}
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
