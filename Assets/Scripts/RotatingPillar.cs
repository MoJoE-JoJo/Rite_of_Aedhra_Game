using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotatingPillar : Lock
{
    private bool isMoving = false;
    private int positions = 4;
    [SerializeField]
    private int onPosition = 0;
    [SerializeField]
    private int currentPosition = 0;
    private float rotationAngle;
    private float startingRotation;
    private float animationTime = 0.5f;
    private float time = 0;

    protected override void Start()
    {
        rotationAngle = 360f / positions;
        startingRotation = transform.eulerAngles.y;
        isOn = onPosition == currentPosition;
    }

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
            currentPosition = (currentPosition + 1) % positions;
            isOn = onPosition == currentPosition;
            OnLockChanged();
            return;
        }
        // animate the lever handle
        Quaternion from = Quaternion.Euler(transform.eulerAngles.x, startingRotation + currentPosition * rotationAngle, transform.eulerAngles.z);
        Quaternion to = Quaternion.Euler(transform.eulerAngles.x, startingRotation + (currentPosition + 1) * rotationAngle, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(from, to, time * 2);
    }

    public void Rotate()
    {
        // check if pillar can rotate
        if (isMoving)
        {
            return;
        }

        // Start rotating the pillar. The on isn't toggled until the movement is finished.
        time = 0;
        isMoving = true;
    }

    override public void InteractWithItem()
    {
        return;
    }
}
