using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 5f;
    Vector3 _offset;

    // Use this for initialization
    void Start () {
        _offset = transform.position - target.position;
    }
	
    // Update is called once per frame
    void LateUpdate () {
        Vector3 targetCamPos = target.position + _offset;
        transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
