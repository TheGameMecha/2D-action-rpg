using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueInteractableEntity2D : InteractableEntity2D
{
    [SerializeField] private DialogueObject dialogue;

    public override void InteractedWith(Entity2D otherEntity)
    {
        base.InteractedWith(otherEntity);
        DialogueManager.instance.StartDialogue(dialogue.dialogue);
    }
}
