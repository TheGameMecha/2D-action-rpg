using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    public Image healthBar;
    SpriteCharacterController controller;
    Character character;

    private void Awake()
    {
        controller = GetComponent<SpriteCharacterController>();
        character = controller.GetCharacter();
    }

    private void Update()
    {
        healthBar.fillAmount = (float)character.currentHealth / (float)character.maximumHealth;
    }
}
