﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion

    public LayerMask interactableLayer;

    public bool isUiActive = false;

    private void Update()
    {
        if (DialogueManager.instance.dialogueIsActive)
            isUiActive = true;
        else
            isUiActive = false;
    }
}

public static class StaticGameElements
{

}