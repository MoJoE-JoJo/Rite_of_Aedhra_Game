using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpDownPillar : MonoBehaviour
{
    [SerializeField]
    private bool isUp = false;
    private bool isMoving = false;
    private float upPosition = -3f; // is set in Start
    private float downPosition = -4f; // is set in Start
    private float positionChange = 1f;
    private float animationTime = 0.5f;
    private float time = 0;

    private void Start()
    {
        upPosition = transform.position.y + (isUp ? 0 : positionChange);
        downPosition = transform.position.y - (isUp ? positionChange : 0);
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
            isUp = !isUp;
            return;
        }
        // animate the pillar
        Vector3 from = new Vector3(transform.position.x, (isUp ? upPosition : downPosition), transform.position.z);
        Vector3 to = new Vector3(transform.position.x, (isUp ? downPosition : upPosition), transform.position.z);
        transform.position = Vector3.Lerp(from, to, time * 2);
    }

    public void TogglePillar(bool up)
    {
        // check if pillar can be toggled
        if (isUp == up)
        {
            if (!isMoving)
            {
                return;
            }
            isUp = !isUp;
        }

        // Start moving the pillar
        time = 0;
        isMoving = true;
    }
}
