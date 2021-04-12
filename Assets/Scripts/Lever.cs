using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lever : Item
{
    [SerializeField]
    private GameObject leverHandle;
    [SerializeField]
    private bool singleUse = true;
    private bool isOn = false;
    private bool isMoving = false;
    private float upPosition = 15f;
    private float downPosition = 80f;
    private float animationTime = 0.5f;
    private float time = 0;


    public delegate void LeverChanged();

    public static event LeverChanged LeverChangedEvent;

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }
        time += Time.deltaTime;
        if (time >= animationTime)
        {
            isMoving = false;
            isOn = !isOn;
            LeverChangedEvent();
            return;
        }
        // animate the lever handle
        Transform handleTransform = leverHandle.GetComponent<Transform>();
        Quaternion from = Quaternion.Euler(transform.eulerAngles.x + (isOn ? downPosition : upPosition), transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion to = Quaternion.Euler(transform.eulerAngles.x + (isOn ? upPosition : downPosition), transform.eulerAngles.y, transform.eulerAngles.z);
        handleTransform.rotation = Quaternion.Lerp(from, to, time * 2);
    }

    override protected void InteractWithItem()
    {
        // check if lever can be toggled
        if ((singleUse && isOn) || isMoving)
        {
            return;
        }

        // Start moving the lever. The on isn't toggled until the movement is finished.
        time = 0;
        isMoving = true;
    }

    public bool GetIsOn()
    {
        return isOn;
    }
}
