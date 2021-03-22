using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copied from Algorithms for Games Development.
public class MoveAlongPath : MonoBehaviour
{
    private CurvePath path;

    private float lapTime = 1.0f;

    [Tooltip("Should the object move?")]
    public bool stop = false;

    public bool parameterizeByArclength = true;
    public bool useEasingCurve = false;

    // we keep an internal clock for this object for higher flexibility
    private float localTime = 0;

    public void AddControlPoints(Vector3[] controlPoints)
    {
        CurvePath p = gameObject.AddComponent<CurvePath>();
        path = p;
        path.AddControlPoints(controlPoints);
        lapTime = 0.1f * (controlPoints[controlPoints.Length - 1] - controlPoints[0]).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null || stop)
            return;

        if (localTime >= lapTime) stop = true;

        localTime += Time.deltaTime;

        float t = localTime / lapTime;

        float s = t;
        if (useEasingCurve)
            s = EasingFunctions.Crossfade(EasingFunctions.SmoothStart2, EasingFunctions.SmoothStop2, 0.5f, t);


        // move this object along the line            
        Vector3 pos = path.Evaluate(s, parameterizeByArclength);
        this.transform.position = pos;

    }
}
