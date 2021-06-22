using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffect : SpriteEffect
{
    public override void OnAnimationFinished()
    {
        base.OnAnimationFinished();
        Destroy(gameObject);
    }
}
