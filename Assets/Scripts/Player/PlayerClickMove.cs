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

    // Start is called before the first frame update
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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
                _agent.SetDestination(info.point);
                pathDrawer.DrawGoal(_agent.destination);
            }
        }

        bool isMoving = !PathComplete();
        _animator.SetBool(Walking, isMoving);
        if (!isMoving) return;
        FaceDirection();
        _animator.SetFloat(SpeedMult, _agent.velocity.magnitude * animSpeedMultiplier);
    }

    public bool PathComplete()
    {
        if (!_agent) return true;
        if (!(_agent.remainingDistance <= _agent.stoppingDistance)) return false;
        return !_agent.hasPath || _agent.velocity.sqrMagnitude == 0f;
    }

    public void StopMoving()
    {
        _agent.ResetPath();
        _animator.SetBool(Walking, false);
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
