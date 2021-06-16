using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Data/Dialogue")]
    public class DialogueObject : ScriptableObject
    {
       public Dialogue dialogue;
    }
}