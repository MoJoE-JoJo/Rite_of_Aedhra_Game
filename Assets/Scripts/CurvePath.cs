using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Copied from Algorithms for Games Development.
/// A path consisting of one or more CurveSegments.
/// </summary>
public class CurvePath : MonoBehaviour
{
    private CurveSegment[] curves;

    // this array is used to store the normalized, cumulative arclength of the path
    // it relates the array index to the arclength of the path, so we can think of it
    // as a table
    private float[] normArclength;
    private int arclengthEntries = 200; // the length of our table/array

    public void AddControlPoints(Vector3[] controlPoints)
    {

        int points = controlPoints.Length;
        curves = new CurveSegment[points - 3];

        for (int i = 0; i + 3 < points; i++)
        {
            Vector3 cp1 = controlPoints[i];
            Vector3 cp2 = controlPoints[i + 1];
            Vector3 cp3 = controlPoints[i + 2];
            Vector3 cp4 = controlPoints[i + 3];

            curves[i] = new CurveSegment(cp1, cp2, cp3, cp4, CurveType.BEZIER);
        }
        // compute the cumulative arclength, which we use to sample the curve
        // with a parameter that is linear with relation to the arclength of the curve
        UpdateArclength();
    }

    /// <summary>
    /// Evaluate the point in the path at parameter 's', 
    /// 's' is transformed to 'u' if arclengthParameterization is set
    /// Evaluate the tangent if derivative==1, and the curvature if derivative==2
    /// </summary>
    /// <param name="s"></param>
    /// <param name="arclengthParameterization"></param>
    /// <param name="derivative"></param>
    /// <returns></returns>
    public Vector3 Evaluate(float s, bool arclengthParameterization = false, int derivative = 0)
    {
        if (curves == null)
            return Vector3.zero;

        float u = arclengthParameterization ? ParameterizeByArclength(s) : s;

        // scale 'u' from [0,1] to [0, curves.Length]
        float pathU = u * curves.Length;
        // round down pathU to retrieve the curve segment ID
        int curveID = (int)pathU;
        // ensure that the curveID is in a valid range
        curveID = Mathf.Clamp(curveID, 0, curves.Length - 1);
        // 'u' in the selected curve segment
        float curveU = pathU - (float)curveID;


        if (derivative == 1)
            return curves[curveID].EvaluateDv(curveU);
        if (derivative == 2)
            return curves[curveID].EvaluateDv2(curveU);

        return curves[curveID].Evaluate(curveU);
    }

    /// <summary>
    /// Parameterize by arclength. Given the table of (normalized) cumulative 
    /// arclengths of the path and a parameter 's', returns a parameter 'u' 
    /// that evaluates the point at s*arclength of the path
    /// </summary>
    /// <param name="s"></param>
    /// <returns>returns the 'u' parameter</returns>
    float ParameterizeByArclength(float s)
    {
        // ensure tha s is in a valid range
        s = Mathf.Clamp(s, 0.0f, 1.0f);

        // we perform a binary search to find the entries in the 
        // table that are closer to 's'
        int totalSegments = normArclength.Length - 1;
        int min = 0;
        int max = totalSegments;
        int current = max / 2;

        while (true)
        {
            // if min and max are neighbours, we found the best approximation
            // of 's' in the table
            if (min == max - 1)
            {
                // table the two best approximations of 's' in the table
                // and compute the closest 'u' with a linear interpolation
                // this means that 'u' is just an approximation of the correct parameter
                float s1 = normArclength[min];
                float s2 = normArclength[max];
                float u1 = (float)min / (float)totalSegments;
                float u2 = (float)max / (float)totalSegments;
                float delta_s = (s - s1) / (s2 - s1);

                return Mathf.Lerp(u1, u2, delta_s); ;
            }

            // adjust the bounds of our search range
            if (s > normArclength[current])
                min = current;
            else
                max = current;

            // index in the middle of our current search range
            current = min + (max - min) / 2;
        }
    }

    /// <summary>
    /// construct a table with the normalized, cumulative arclength of the curve
    /// </summary>
    void UpdateArclength()
    {
        // compute entry intervals so that interval * arclengthEntries == 1
        float interval = 1.0f / (float)arclengthEntries;

        // table initialization
        normArclength = new float[arclengthEntries];
        normArclength[0] = 0.0f;

        // start position in the path
        Vector3 lastPos = Evaluate(0, false);

        for (int i = 1; i < arclengthEntries; i++)
        {
            // sample path at fixed intervals
            float u = interval * (float)i;
            Vector3 pos = Evaluate(u, false);

            // compute the distance between two consecutive samples and 
            // accumulate in arclength
            float s = normArclength[i - 1] + Vector3.Distance(lastPos, pos);
            normArclength[i] = s;

            lastPos = pos;
        }
        // now the entries in normArclength are in the range [0, pathArclenght]

        // the last element in normArclength is the total arclength of the path
        float pathArclenght = normArclength[normArclength.Length - 1];

        // ensure there is no division by 0, 
        // could happen if all control points are in the same place
        if (pathArclenght <= float.Epsilon)
            throw new System.Exception("totalArclength must be positive and bigger than 0");

        // normalise the arclength values by dividing by the last element
        for (int i = 1; i < arclengthEntries; i++)
        {
            normArclength[i] = normArclength[i] / pathArclenght;
        }
        // now the entries in normArclength are in the range [0, 1]
    }
}
