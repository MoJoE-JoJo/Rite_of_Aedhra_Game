using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    // Update is called once per frame
    [SerializeField] private new Camera camera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    // [SerializeField] private float speed = 3f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSpeed = 10f;

    private float _turnSpeed;
    private NavMeshAgent _navMeshAgent;

    private static readonly int Walking = Animator.StringToHash("Walking");

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float zDir = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(xDir, 0f, zDir);
        Transform transform1 = camera.transform;
        if (dir.magnitude >= 0.1f) 
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSpeed, turnSmoothTime);
            dir = transform1.TransformDirection(new Vector3(dir.x, 0f, dir.z));
            dir.y = 0;
            Debug.Log(dir.normalized);
            _navMeshAgent.SetDestination(transform.position + dir.normalized);
            FaceDirection();
            animator.SetBool(Walking, true);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            animator.SetBool(Walking, false);
        }
    }
    
    private void FaceDirection()
    {
        if (_navMeshAgent.velocity.sqrMagnitude == 0f) return;

        Vector3 direction = _navMeshAgent.velocity.normalized;
        direction.y = 0;
        Quaternion qDir = Quaternion.LookRotation(direction);
        _navMeshAgent.transform.rotation = Quaternion.Slerp(_navMeshAgent.transform.rotation, qDir, Time.deltaTime * turnSpeed);
    }
}
