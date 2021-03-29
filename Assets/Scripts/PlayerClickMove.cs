using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class PlayerClickMove : MonoBehaviour
{

    [Range(0f, 1f)]
    [SerializeField] private float animSpeedMultiplier = 0.5f;
    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask pathMask;
    [SerializeField] private float lookAtSpeed = 10f;
    [SerializeField] private DrawPlayerPath pathDrawer;


    private NavMeshAgent _agent;
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int SpeedMult = Animator.StringToHash("speedMult");

    // items
    private ItemType heldItem;
    private GameObject rockPrefab;
    [SerializeField]
    private GameObject rockCooldown;

    // Start is called before the first frame update
    private void Awake()
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
    }

    // Update is called once per frame
    private void Update()
    {
        if(PathComplete())
            pathDrawer.HideGoal();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
    
            if (Physics.Raycast(ray, out info, 100, pathMask))
            {
                _agent.SetDestination(info.point);
                pathDrawer.DrawGoal(_agent.destination);
            }
        }

        // Throw rock
        if (Input.GetKeyDown("space"))
        {
            heldItem = ItemType.ROCK; // Debug
            if (heldItem == ItemType.ROCK)
            {
                ThrowRock();
            }
            heldItem = ItemType.NONE;
        }

        bool isMoving = !PathComplete();
        _animator.SetBool(Walking, isMoving);
        if (!isMoving) return;
        FaceDirection();
        _animator.SetFloat(SpeedMult, _agent.velocity.magnitude * animSpeedMultiplier);
    }

    private void ThrowRock()
    {
        // start cooldown, if false it is not ready yet.
        if (!rockCooldown.GetComponent<RockCooldown>().StartCooldown())
        {
            return;
        }
        // stop current movement?
        // play throw animation
        // create the rock
        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 origin = transform.position + offset;
        GameObject go = Instantiate(rockPrefab, origin, Quaternion.identity);
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        // find the rock throws end location
        // should improve this by ignoring certain object types (trees etc.)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        Vector3 target = hit.point;

        // from https://forum.unity.com/threads/how-to-calculate-force-needed-to-jump-towards-target-point.372288/
        float initialAngle = 40f;
        Rigidbody rigid = go.AddComponent<Rigidbody>();
        rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Vector3 p = target;
            
        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(origin.x, 0, origin.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = origin.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        Vector3 direction = (planarTarget - planarPostion);
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, direction);
        if (direction.x < 0)
        {
            angleBetweenObjects = 360 - angleBetweenObjects;
        }
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        rigid.velocity = finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
        
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
    public bool PathComplete()
    {
        if (!_agent) return true;
        if (!(_agent.remainingDistance <= _agent.stoppingDistance)) return false;
        return !_agent.hasPath || _agent.velocity.sqrMagnitude == 0f;
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
