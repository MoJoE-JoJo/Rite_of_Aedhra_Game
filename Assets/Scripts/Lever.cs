using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lever : Lock
{
    [SerializeField]
    private GameObject leverHandle;
    [SerializeField]
    private bool singleUse = true;
    private bool isMoving = false;
    private float upPosition = 15f;
    private float downPosition = 80f;
    private float animationTime = 0.5f;
    private float time = 0;

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
            OnLockChanged();
            return;
        }
        // animate the lever handle
        Transform handleTransform = leverHandle.GetComponent<Transform>();
        Quaternion from = Quaternion.Euler(transform.eulerAngles.x + (isOn ? downPosition : upPosition), transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion to = Quaternion.Euler(transform.eulerAngles.x + (isOn ? upPosition : downPosition), transform.eulerAngles.y, transform.eulerAngles.z);
        handleTransform.rotation = Quaternion.Lerp(from, to, time * 2);
    }

    override public void InteractWithItem()
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
}
