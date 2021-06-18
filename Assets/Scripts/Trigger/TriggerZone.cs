using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerZone : MonoBehaviour
{
    [SerializeField] private EntityType allowedAvatarType;
    [SerializeField] bool triggerOnlyOnce = false;

    Collider2D col;
    bool hasBeenTriggered = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOnlyOnce && hasBeenTriggered)
            return;

        if (collision.GetComponent<Entity2D>())
        {
            if (ReturnSelectedElements().Contains((int)collision.GetComponent<Entity2D>().entityType))
            {
                if (triggerOnlyOnce && hasBeenTriggered == false)
                    hasBeenTriggered = true;

                ExecuteOnEnter(collision.GetComponent<Entity2D>());
            }
        }
    }

    public virtual void ExecuteOnEnter(Entity2D otherEntity)
    {
        Debug.Log("Trigger: " + gameObject.name + " activated by " + otherEntity.name);
    }

    List<int> ReturnSelectedElements()
    {
        List<int> selectedElements = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(EntityType)).Length; i++)
        {
            int layer = 1 << i;
            if (((int)allowedAvatarType & layer) != 0)
            {
                selectedElements.Add(i);
            }
        }

        return selectedElements;
    }
}