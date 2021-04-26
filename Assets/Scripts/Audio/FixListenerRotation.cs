using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FixListenerRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        Transform transform1 = transform;
        Vector3 eulerAngles = transform1.eulerAngles;
        Vector3 angles = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera.transform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, angles.y,
            eulerAngles.z);
        transform1.eulerAngles = new Vector3(eulerAngles.x, angles.y,
            eulerAngles.z);;
    }
}
