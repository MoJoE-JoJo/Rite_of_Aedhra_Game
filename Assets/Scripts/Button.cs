using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Button : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonMoveable;
    [SerializeField]
    private GameObject pillar;
    private bool isMovingOut = false;
    private bool isMoving = false;
    private float outPosition = 0f;
    private float inPosition = -1.8f;
    private float animationTime = 0.4f;
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
            if (!isMovingOut)
            {
                isMovingOut = true;
                time = 0;
                pillar.GetComponent<RotatingPillar>().Rotate();
            } else
            {
                isMoving = false;
                isMovingOut = false;
                return;
            }
        }
        // animate the buttonMoveable
        Transform handleTransform = buttonMoveable.GetComponent<Transform>();
        Quaternion from = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isMovingOut ? inPosition : outPosition), transform.eulerAngles.z);
        Quaternion to = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + (isMovingOut ? outPosition : inPosition), transform.eulerAngles.z);
        handleTransform.rotation = Quaternion.Lerp(from, to, time * 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if button can be pressed
        if (isMoving)
        {
            return;
        }

        // Start moving the buttonMovable. The event is triggered when it has moved all the way in.
        time = 0;
        isMoving = true;
    }
}
