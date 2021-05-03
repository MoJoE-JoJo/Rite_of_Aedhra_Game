using System;
using System.Collections;
using System.Collections.Generic;
using Game_Systems;
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


    public NavMeshAgent Agent { get; private set; }
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int SpeedMult = Animator.StringToHash("speedMult");

    // Start is called before the first frame update
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        GameManager gm = GameManager.Instance;
        if (gm.spawnPoint == Vector3.zero)
            gm.spawnPoint = transform.position;
        else
            WarpToPoint(gm.spawnPoint);
        if (gm.spawnRot == Quaternion.identity)
            gm.spawnRot = transform.rotation;
        else
            transform.rotation = gm.spawnRot;
    }

    public void WarpToPoint(Vector3 point)
    {
        NavMesh.SamplePosition(point, out NavMeshHit hit, Agent.height*2, NavMesh.AllAreas);
        if(hit.hit) {
            Agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("No hits!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(PathComplete())
            pathDrawer.HideGoal();
        if (GameManager.AllowInput && Input.GetMouseButton(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
    
            if (Physics.Raycast(ray, out info, 100, pathMask))
            {
                Agent.SetDestination(info.point);
                pathDrawer.DrawGoal(Agent.destination);
            }
        }

        bool isMoving = !PathComplete();
        _animator.SetBool(Walking, isMoving);
        if (!isMoving) return;
        FaceDirection();
        _animator.SetFloat(SpeedMult, Agent.velocity.magnitude * animSpeedMultiplier);
    }

    public bool PathComplete()
    {
        if (!Agent) return true;
        if (!(Agent.remainingDistance <= Agent.stoppingDistance)) return false;
        return !Agent.hasPath || Agent.velocity.sqrMagnitude == 0f;
    }

    public void StopMoving()
    {
        Agent.ResetPath();
        _animator.SetBool(Walking, false);
    }

    private void FaceDirection()
    {
        if (Agent.velocity.sqrMagnitude == 0f) return;

        Vector3 direction = Agent.velocity.normalized;
        direction.y = 0;
        Quaternion qDir = Quaternion.LookRotation(direction);
        Agent.transform.rotation = Quaternion.Slerp(Agent.transform.rotation, qDir, Time.deltaTime * lookAtSpeed);
    }
}
