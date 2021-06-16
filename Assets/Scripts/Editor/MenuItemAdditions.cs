using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuItemAdditions : Editor
{
    [MenuItem("GameObject/Create Other/Entity2D")]
    public static void CreateEntity2D()
    {
        GameObject go = new GameObject();
        go.name = "New Entity2D";
        go.AddComponent<BoxCollider2D>();
        go.AddComponent<Entity2D>();
        Selection.activeGameObject = go;
    }

    [MenuItem("GameObject/Create Other/Trigger Zone")]
    public static void CreateTriggerZone()
    {
        GameObject go = new GameObject();
        go.name = "New Trigger Zone";
        go.AddComponent<BoxCollider2D>().isTrigger = true;
        go.AddComponent<TriggerZone>();
        Selection.activeGameObject = go;
    }

    [MenuItem("GameObject/Create Other/Dialogue Trigger Zone")]
    public static void CreateDialogueTriggerZone()
    {
        GameObject go = new GameObject();
        go.name = "New Dialogue Trigger Zone";
        go.AddComponent<BoxCollider2D>().isTrigger = true;
        go.AddComponent<DialogueTriggerZone>();
        Selection.activeGameObject = go;
    }

    [MenuItem("GameObject/Create Other/Sprite Character")]
    public static void CreateSpriteCharacterController()
    {
        GameObject go = new GameObject();
        go.name = "New Sprite Character Controller";
        go.AddComponent<BoxCollider2D>();
        go.AddComponent<Rigidbody2D>();
        go.AddComponent<SpriteCharacterController>();
        Selection.activeGameObject = go;
    }
}
