using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (Animator))]
public class MoveAnimationManager : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private Vector2 _smoothDeltaPosition = Vector3.zero;
    private Vector2 _velocity = Vector3.zero;
    private static readonly int Walking = Animator.StringToHash("Walking");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     Transform transform1 = transform;
    //     Vector3 worldDeltaPosition = _agent.nextPosition - transform1.position;
    //     
    //     // map to local space
    //     float dx = Vector3.Dot(transform1.right, worldDeltaPosition);
    //     float dy = Vector3.Dot(transform1.forward, worldDeltaPosition);
    //     var deltaPosition = new Vector2(dx, dy);
    //     
    //     // low-pass filter the deltamove
    //     float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
    //     _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);
    //     if (Time.deltaTime > 1e-5f)
    //         _velocity = _smoothDeltaPosition / Time.deltaTime;
    //     bool shouldMove = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.radius;
    //     _animator.SetBool(Walking, shouldMove);
    //     // _animator.SetFloat();
    // }
}
