using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask pathableMask;
    [SerializeField] private float lookAtSpeed = 10;

    private NavMeshAgent _agent;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");

    // items
    private ItemType heldItem;
    private GameObject rockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        rockPrefab = (GameObject)Resources.Load("Prefabs/SM_Env_Rock_014.prefab", typeof(GameObject)); // Can't figure out why, but this aint working.

        // The below loop should be removed, using it temporarily to get the prefab until the above Resources.Load is fixed.
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as UnityEngine.Object[])
        {
            if (go.name == "SM_Env_Rock_014")
            {
                rockPrefab = go;
            }
        }
        Debug.Log(rockPrefab);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;

            if (Physics.Raycast(ray, out info, 100, pathableMask))
            {
                _agent.SetDestination(info.point);
            }
        }

        // Throw rock
        if (Input.GetKeyDown("space"))
        {
            heldItem = ItemType.ROCK; //Debug
            if (heldItem == ItemType.ROCK)
            {
                ThrowRock();
            }
        }

        FaceDirection();
        _animator.SetBool(Walking, !PathComplete());
    }

    private void ThrowRock()
    {
        // stop current movement?
        // play throw animation
        // create the rock
        Vector3 startP = gameObject.GetComponent<Transform>().position + new Vector3(0, 2, 0);
        GameObject go = Instantiate(rockPrefab, startP, Quaternion.identity);
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        // find the rock throws end location
        // should improve this by ignoring certain object types (trees etc.)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        Vector3 endP = hit.point;
        // create path to move the rock along using bezier curve
        float dist = (endP - startP).magnitude;
        Vector3 p2 = startP + new Vector3(0, dist, 0) / 2;
        Vector3 p3 = endP + new Vector3(0, dist, 0) / 2;
        CurveSegment curve = new CurveSegment(startP, p2, p3, endP, CurveType.BEZIER);
        // debug show curve
        DrawCurveSegments(curve);
        // move the rock along the path at a realistic speed (done in MoveAlongPath script)
        var map = go.AddComponent<MoveAlongPath>();
        map.AddControlPoints(new Vector3[] { startP, p2, p3, endP});
    }

    private void DrawCurveSegments(CurveSegment curve, int segments = 50)
    {
        float interval = 1.0f / segments;
        Vector3 lastPos = curve.Evaluate(0);
        for (int i = 1; i <= segments; i++)
        {
            float u = interval * (float)i;
            Vector3 pos = curve.Evaluate(u);
            DrawLine(lastPos, pos, 5f);
            lastPos = pos;
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, float duration = 0.2f)
    {
        GameObject go = new GameObject();
        go.transform.position = start;
        go.AddComponent<LineRenderer>();
        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(go, duration);
    }

    protected bool PathComplete()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

    private void FaceDirection()
    {
        if (_agent.velocity.sqrMagnitude == 0f) return;

        Vector3 direction = _agent.velocity.normalized;
        direction.y = 0;
        Quaternion qDir = Quaternion.LookRotation(direction);
        _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation, qDir, Time.deltaTime * lookAtSpeed);
    }
}
