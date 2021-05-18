using Game_Systems;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [RequireComponent(typeof (NavMeshAgent))]
    public class PlayerClickMove : MonoBehaviour
    {
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float runningSpeed;
        [SerializeField] private float staminaDrainMult = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float animSpeedMultiplier = 0.5f;
        [SerializeField] private new Camera camera;
        [SerializeField] private LayerMask pathMask;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private DrawPlayerPath pathDrawer;
        [SerializeField] private StaminaBar staminaBar;
        [SerializeField] private float interactRange = 2f;

        private GameObject interactTarget;

        public bool isRunning;
        public NavMeshAgent Agent { get; private set; }
        private Animator _animator;
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int SpeedMult = Animator.StringToHash("speedMult");
        private static readonly int Running = Animator.StringToHash("running");

        // Start is called before the first frame update
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }
    
        private void Start()
        {
            staminaBar = FindObjectOfType<StaminaBar>();
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
            if(hit.hit)
                Agent.Warp(hit.position);
            else
                Debug.LogError("No hits!");
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (interactTarget)
            {
                if ((interactTarget.transform.position - transform.position).magnitude <= interactRange)
                {
                    interactTarget.GetComponent<Item>().InteractWithItem();
                    interactTarget = null;
                }
            }
            if (!GameManager.AllowInput)
                return;
            if(PathComplete())
                pathDrawer.HideGoal();
            isRunning = Input.GetKey(KeyCode.LeftShift);
            if(Input.GetMouseButton(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit info;

                if (Physics.Raycast(ray, out info, 100, interactableMask))
                {
                    if ((info.transform.position - transform.position).magnitude <= interactRange)
                    {
                        info.transform.gameObject.GetComponent<Item>().InteractWithItem();
                    }
                    else
                    {
                        Agent.SetDestination(info.point);
                        pathDrawer.DrawGoal(Agent.destination);
                        interactTarget = info.transform.gameObject;
                    }
                }
                else if (Physics.Raycast(ray, out info, 100, pathMask))
                {
                    Agent.SetDestination(info.point);
                    pathDrawer.DrawGoal(Agent.destination);
                    interactTarget = null;
                }
            }

            bool isMoving = !PathComplete();
            if (!isMoving)
            {
                _animator.SetBool(Walking, false);
                _animator.SetBool(Running, false);
                return;
            }

            bool run = isRunning && staminaBar.UseStamina(0.4f * staminaDrainMult);
            _animator.SetBool(Walking, !run);
            _animator.SetBool(Running, run);
            Agent.speed = run ? runningSpeed : walkingSpeed;

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
            _animator.SetBool(Running, false);
        }

        private void FaceDirection()
        {
            if (Agent.velocity.sqrMagnitude == 0f) return;
            Vector3 direction = Agent.velocity.normalized;
            direction.y = 0;
            if (direction == Vector3.zero) return;
            Quaternion qDir = Quaternion.LookRotation(direction);
            Agent.transform.rotation = Quaternion.Slerp(Agent.transform.rotation, qDir, Time.deltaTime * 20f);
        }
        
        public void FaceTarget(Vector3 target)
        {
            Quaternion qDir = Quaternion.LookRotation(new Vector3(target.x, 0, target.z));
            Agent.transform.rotation = Quaternion.Slerp(Agent.transform.rotation, qDir, Time.deltaTime * 10f);
        }
    }
}
