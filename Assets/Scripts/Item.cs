using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = GetComponent<Renderer>().material.color;
        cursorTexture = Resources.Load<Texture2D>("Cursors/DemoCursor.png"); // Can't figure out why, but this aint working.
        Debug.Log(cursorTexture);

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
        this.SetHighlightActive(true);
        this.SetPickupCursorActive(true);
    }

    void OnMouseExit()
    {
        this.SetHighlightActive(false);
        this.SetPickupCursorActive(false);
    }

    void SetHighlightActive(bool active)
    {
        if (active)
        {
            rend.material.color = Color.red;
        } else
        {
            rend.material.color = originalColor;
        }
        
    }

    void SetPickupCursorActive(bool active)
    {
        if (active)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        } else
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }
}
