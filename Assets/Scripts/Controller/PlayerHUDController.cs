using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    public Image healthBacker;
    public Image healthBar;
    SpriteCharacterController controller;
    Character character;

    private void Awake()
    {
        controller = GetComponent<SpriteCharacterController>();
        character = controller.GetCharacter();
        SetHPBarSize();
    }

    private void Update()
    {
        healthBar.fillAmount = (float)character.currentHealth / (float)character.maximumHealth;
    }

    public void SetHPBarSize()
    {
        healthBacker.rectTransform.sizeDelta = new Vector2(character.maximumHealth, 5);
        healthBar.rectTransform.sizeDelta = new Vector2(character.maximumHealth, 5);
    }
}
