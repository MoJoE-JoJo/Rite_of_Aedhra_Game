using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    NONE,
    ROCK,
    TORCH,
    KEY,
    BOULDER
}

public class Item : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    private Color highlightColor = Color.red;
    private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    public ItemType type;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = GetComponent<Renderer>().material.color;
        cursorTexture = Resources.Load<Texture2D>("Cursors/DemoCursor.png"); // Can't figure out why, but this aint working.
        // Debug.Log(cursorTexture);

        // The below loop should be removed, using it temporarily to get the texture until the above Resources.Load is fixed.
        foreach (Texture2D t2 in Resources.FindObjectsOfTypeAll(typeof(Texture2D)) as UnityEngine.Object[])
        {
            if (t2.name == "DemoCursor")
            {
                cursorTexture = t2;
            }
        }
    }

    void OnMouseEnter()
    {
        SetHighlightActive(true);
        SetPickupCursorActive(true);
    }

    void OnMouseExit()
    {
        SetHighlightActive(false);
        SetPickupCursorActive(false);
    }

    void SetHighlightActive(bool active)
    {
        if (active)
        {
            rend.material.color = highlightColor;
        }
        else
        {
            rend.material.color = originalColor;
        }

    }

    void SetPickupCursorActive(bool active)
    {
        if (active)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    ItemType PickUpItem()
    {
        // Should the item be displayed in the player character's hand? Otherwise just destroy it here.
        Destroy(gameObject);
        return type;
    }

    ItemType InteractWithItem()
    {
        throw new NotImplementedException();
        // return type;
    }

    // This instantly picks up the item regardless of player position. Needs to be limited based on the player characters position.
    // Might also need to support move to, then pick up.
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                switch (type)
                {
                    case ItemType.ROCK:
                        {
                            PickUpItem();
                            break;
                        }
                    case ItemType.BOULDER:
                        {
                            InteractWithItem();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}

