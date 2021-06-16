using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Entity2D that can be interacted with by the player pressing the "A" button
/// Functionality for this is handled in the PlayerInputController
/// </summary>
public class InteractableEntity2D : Entity2D
{
    public virtual void InteractedWith()
    {

    }
}
