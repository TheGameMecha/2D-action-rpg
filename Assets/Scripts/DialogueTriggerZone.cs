using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class DialogueTriggerZone : TriggerZone
{

    [SerializeField] private DialogueObject dialogue;

    public override void ExecuteOnEnter(Entity2D otherEntity)
    {
        DialogueManager.instance.StartDialogue(dialogue.dialogue);
    }
}
