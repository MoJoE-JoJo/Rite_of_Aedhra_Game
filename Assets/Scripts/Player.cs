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

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;

            if (Physics.Raycast(ray, out info, 100, pathableMask)) ;
            {
                _agent.SetDestination(info.point);
            }
        }
        FaceDirection();
        _animator.SetBool(Walking, !PathComplete());
    }
    
    protected bool PathComplete()
    {
        if ( _agent.remainingDistance <= _agent.stoppingDistance)
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


