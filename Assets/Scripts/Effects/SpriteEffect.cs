using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public virtual void OnAnimationFinished()
    {

    }
}
