using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private LockPosition[] lockPositions;
    [SerializeField]
    private GameObject door;
    private bool isOpen = false;
    private bool isMoving = false;
    private float closedPosition = 90f;
    private float openPosition = 0f;
    private float animationTime = 0.5f;
    private float time = 0;

    void Update()
    {
        if (!isMoving)
        {
            return;
        }
        time += Time.deltaTime;
        if (time >= animationTime)
        {
            isMoving = false;
            isOpen = !isOpen;
            return;
        }
        // animate the lever handle. Currently doesn't handle cases where isOpen is changed again before the animation is finished (it always starts at the fully open/closed position).
        Transform handleTransform = door.GetComponent<Transform>();
        Quaternion from = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isOpen ? closedPosition : openPosition), transform.eulerAngles.z);
        Quaternion to = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isOpen ? openPosition : closedPosition), transform.eulerAngles.z);
        handleTransform.rotation = Quaternion.Lerp(from, to, time * 2);
    }

    private void ToggleDoor(bool open)
    {
        // check if door can be toggled
        if (isOpen == open)
        {
            if (!isMoving)
            {
                return;
            }
            isOpen = !isOpen;
        }

        // Start opening the door
        time = 0;
        isMoving = true;
    }

    private void CheckConditions()
    {
        ToggleDoor(ShouldBeOpen());
    }

    private bool ShouldBeOpen()
    {
        foreach(LockPosition lp in lockPositions)
        {
            Lever lever = lp.go.GetComponent<Lever>();
            if (!lever)
            {
                Debug.LogError("GameObject has no Lever component.");
                return false;
            } 
            if (lp.isOn != lever.GetIsOn())
            {
                return false;
            }
        }
        return true;
    }

    void OnEnable()
    {
        Lever.LeverChangedEvent += CheckConditions;
    }

    void OnDisable()
    {
        Lever.LeverChangedEvent -= CheckConditions;
    }
}

[System.Serializable]
public class LockPosition
{
    public GameObject go;
    public bool isOn;
}
