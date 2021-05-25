using System.Collections;
using DG.Tweening;
using Game_Systems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    public class PlayerController : MonoBehaviour

    {
        private PlayerClickMove _clickMoveScript;

        // items
        public GameObject rockPrefab;
        [SerializeField]
        private GameObject rockCooldown;

        private int rockCounter = 0;
        private static readonly int Die = Animator.StringToHash("die");
        private Animator _animator;
        private CapsuleCollider _capsuleCollider;
        [SerializeField] private AudioClip dyingSfx;
        [SerializeField] private CanvasGroup deathScreen;
        private AudioSource _audioSource;
        private Vector3 _rockTarget = Vector3.zero;
        private static readonly int Throw = Animator.StringToHash("throw");
        public bool IsDying { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            GameManager.Instance.EnableInput();
            _audioSource = GetComponent<AudioSource>();
            _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            _animator = gameObject.GetComponent<Animator>();
            // get rock prefab
            //rockPrefab = (GameObject)Resources.Load("Prefabs/SM_Env_Rock_014.prefab", typeof(GameObject)); // Can't figure out why, but this aint working.
            
            // The below loop should be removed, using it temporarily to get the prefab until the above Resources.Load is fixed.

            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as UnityEngine.Object[])
            {
                if (go.name == "SM_Env_Rock_014")
                {
                    rockPrefab = go;
                }
            }

            _clickMoveScript = GetComponent<PlayerClickMove>();
            Assert.IsTrue(_clickMoveScript != null);
        }
        
        // Update is called once per frame
        private void Update()
        {
            // Throw rock
            if (Input.GetKeyDown("r"))
            {
                StartThrowing();
            }
        }

        public void KillPlayer()
        {
            StartCoroutine(DeathSequence(3f));
        }
    
        IEnumerator DeathSequence(float duration)
        {
            GameManager.Instance.DisableInput();
            _clickMoveScript.StopMoving();
            IsDying = true;
            GameManager.Instance.playerIsDead = true;
            _animator.SetTrigger(Die);
            yield return new WaitForSeconds(0.3f);
            _audioSource.PlayOneShot(dyingSfx);
            yield return new WaitForSeconds(duration);
            // deathScreen.enabled = true;
            deathScreen.DOFade(1f, 2f);
            yield return new WaitForSeconds(4f);
            StartCoroutine(RespawnSequence());
        }

        IEnumerator RespawnSequence()
        {
            _clickMoveScript.WarpToPoint(Checkpoint.GetActiveCheckpointPosition());
            _animator.Play("Idle");
            deathScreen.DOFade(0f, 2f);
            yield return new WaitForSeconds(2f);
            // deathScreen.enabled = false;
            GameManager.Instance.EnableInput();
            IsDying = false;
            GameManager.Instance.playerIsDead = false;
            yield return null;
        }
        
        public void ResumeInput()
        {
            print("Must've been the wind.");
            GameManager.Instance.EnableInput();
        }

        public void StopInput()
        {
            print("Stop right there, criminal scum!");
            _clickMoveScript.StopMoving();
            GameManager.Instance.DisableInput();
        }

        private void StartThrowing()
        {
            // check if rockcooldown is added
            if (!rockCooldown)
            {
                return;
            }

            // start cooldown, if false it is not ready yet.
            if (!rockCooldown.GetComponent<RockCooldown>().StartCooldown())
            {
                return;
            }
            print("I'll put some dirt in your eye.");
            // stop current movement?
            // play throw animation
            // create the rock
            _animator.SetTrigger(Throw);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100);
            _clickMoveScript.FaceTarget(hit.point);
            _rockTarget = hit.point;
            transform.LookAt(hit.point);
        }
        
        public void ThrowRock()
        {
            Vector3 target = _rockTarget;
            Vector3 offset = new Vector3(0, 2, 0);
            Vector3 origin = transform.position + offset;
            GameObject go = Instantiate(rockPrefab, origin, Quaternion.identity);
            go.name = go.name + rockCounter++;
            go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            // find the rock throws end location
            // should improve this by ignoring certain object types (trees etc.)

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
    }
}
