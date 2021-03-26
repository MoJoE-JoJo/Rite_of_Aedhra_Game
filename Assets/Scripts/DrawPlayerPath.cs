using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class DrawPlayerPath : MonoBehaviour
{
    public bool showGoal = true;
    public bool showPath = true;
    private LineRenderer _pathLine;
    [SerializeField] private GameObject goalMarker;

    // Start is called before the first frame update
    private void Awake()
    {
        _pathLine = GetComponent<LineRenderer>();
        // if(!_lineRenderer)
        //     throw new SystemException("LineRenderer component required.");
    }
    
    public void DrawGoal(Vector3 destination)
    {
        goalMarker.SetActive(showGoal);
        goalMarker.transform.position = destination;
        Vector3 lookDir = gameObject.transform.position - destination;
        lookDir.y = 0;
        goalMarker.transform.rotation = quaternion.LookRotation(lookDir, Vector3.up);
    }

    public void HideGoal()
    {
        goalMarker.SetActive(false);   
    }

    public void DrawPath(Vector3[] corners)
    {
        if (!showPath) return;
    }
    
    public void HidePath()
    {
        if (!showPath) return;
    }
    private void OnDrawGizmos()
    {
        // if (!_moveScript) return;
        // if (_moveScript.PathComplete()) return;
        // Gizmos.color = new Color(1, 0, 0, 0.5f);
        // Gizmos.DrawCube(_moveScript.Agent.destination, new Vector3(1, 1, 1));
        // var corners = _moveScript.Agent.path.corners;
        // Gizmos.color = Color.blue;
        // for (int i = 0; i < corners.Length - 1; i++)
        // { 
        //     Gizmos.DrawLine(corners[i], corners[i+1]);
        // }
    }
}
