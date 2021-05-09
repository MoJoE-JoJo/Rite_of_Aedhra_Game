using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private LockPosition[] lockPositions;
    [SerializeField]
    private GameObject doorLeft;
    [SerializeField]
    private GameObject doorRight;
    [SerializeField]
    private float animationTime = 0.5f;
    private bool isOpen = false;
    private bool isMoving = false;
    private float closedPosition = 0f;
    private float openPosition = 90f;
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

        // animate the door. Currently doesn't handle cases where isOpen is changed again before the animation is finished (it always starts at the fully open/closed position).
        if (doorLeft)
        {
            Transform handleTransform = doorLeft.GetComponent<Transform>();
            Quaternion from = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isOpen ? openPosition : closedPosition), transform.eulerAngles.z);
            Quaternion to = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isOpen ? closedPosition : openPosition), transform.eulerAngles.z);
            handleTransform.rotation = Quaternion.Lerp(from, to, time / animationTime);
        }
        if (doorRight)
        {
            Transform handleTransform = doorRight.GetComponent<Transform>();
            Quaternion from = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isOpen ? -openPosition : closedPosition), transform.eulerAngles.z);
            Quaternion to = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isOpen ? closedPosition : -openPosition), transform.eulerAngles.z);
            handleTransform.rotation = Quaternion.Lerp(from, to, time / animationTime);
        }
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
            Lock l = lp.go.GetComponent<Lock>();
            if (!l)
            {
                Debug.LogError("GameObject has no Lock component.");
                return false;
            } 
            if (lp.isOn != l.GetIsOn())
            {
                return false;
            }
        }
        return true;
    }

    void OnEnable()
    {
        Lock.LockChangedEvent += CheckConditions;
    }

    void OnDisable()
    {
        Lock.LockChangedEvent -= CheckConditions;
    }
}

[System.Serializable]
public class LockPosition
{
    public GameObject go;
    public bool isOn;
}
