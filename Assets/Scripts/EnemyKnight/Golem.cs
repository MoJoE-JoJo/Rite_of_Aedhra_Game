using Game_Systems;
using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Golem : MonoBehaviour
{

    public Animator anim;
    public Sensor fovSensor;
    public Rigidbody rigidBody;
    public RangeSensor rangeSensor;
    public SteeringRig steeringRig;
    public MeshCollider eyes;
    public GolemCollision golemCollider;
    public bool chasingThrowable;
    public bool chasingPlayer;
    public Shout sfx;
    
    // control animation speed
    [Range(0.0f, 2.0f)]
    [SerializeField] private float chaseAnimationSpeed = 1.0f;
    [Range(0.0f, 2.0f)]
    [SerializeField] private float walkAnimationSpeed = 1.0f;

    public float maxSpeed = 3.5f;
    public float acceleration = 0.4f;
    public float speed = 1.5f;
    private bool _attacking;
    private bool _firstDetect = true;
    private static readonly int SpeedMultiplier = Animator.StringToHash("speedMultiplier");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsChasing = Animator.StringToHash("isChasing");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private HurtBox _hurtBox;

    void Start()
    {
        sfx = GetComponentInChildren<Shout>();

        anim.GetComponent<Animator>();
        eyes.GetComponent<MeshCollider>();
        anim.Update(Random.value);
        _hurtBox = gameObject.GetComponentInChildren<HurtBox>();

    }

    void Update()
    {
        Walk();
        FOVSensor();
        RangeSensor();
        _hurtBox.isEnabled = ChasingPlayer();

    }

    void LateUpdate()
    {
        var transformRotation = transform.rotation;

        transformRotation.x = 0;
        transformRotation.z = 0;

        transform.rotation = transformRotation;

    }
    
    void FOVSensor()
    {
        var detected = fovSensor.GetNearest();

        if (detected != null)
        {
            if (!_attacking)
            {
                if(_firstDetect)
                    sfx.PlaySpottedSfx();
                
                chasingPlayer = true;

                ChaseTarget(detected);

                steeringRig.DestinationTransform = detected.gameObject.transform;

                if (golemCollider.CollisionStatus())
                {
                    StartCoroutine(Attacking());
                }
                else
                {
                    StopAllCoroutines();
                }

                _firstDetect = false;
            }
            else
            {
                golemCollider.SetCollisionStatus(false);      
                _firstDetect = true;
            }
        }
        else
        {
            Walk();
            if (chasingPlayer)
            {
                if (GameManager.Instance.Player.GetComponent<PlayerController>().IsDying)
                    sfx.PlayKillSfx();
                else
                    sfx.PlayLostSightSfx();
            }

            chasingPlayer = false;

            ResetSpeed();
        }
    }

    void RangeSensor()
    {
        if (chasingPlayer) return; // not get distracted by rocks while chasing
        var detected = rangeSensor.GetNearest();
        var rock = detected?.GetComponent<RockTest>();

        if (detected != null && rock != null && rock.GetRockStatus())
        {
            if (!_attacking)
            {              
                ChaseTarget(detected);

                steeringRig.DestinationTransform = detected.gameObject.transform;

                chasingThrowable = true; 
            }
        }
        else
        {
            Walk();

            chasingThrowable = false;
        }
    }

    private void ResetSpeed()
    {
        speed = 1.0f;
    }

    public void ForceChasePlayer(GameObject player)
    {
       transform.LookAt(player.transform);
    }

    private void ChaseTarget(GameObject target)
    {     
          transform.LookAt(target.transform);
        if (chasingThrowable)
        {
            speed = 1.5f;
            Walk(speed);
        }
        if (chasingPlayer)
        {
            if (speed <= maxSpeed)
                speed += acceleration * Time.deltaTime;
            Chase(speed);
        }      

        Transform transformVar = transform;
        transformVar.position += transformVar.forward * (speed * Time.deltaTime);
        anim.SetFloat(SpeedMultiplier, speed * chaseAnimationSpeed);
        SteerTowardsTarget(target);
        //
        // Debug.Log(speed);
    }

    public bool ChasingPlayer()
    {
        return chasingPlayer;
    }

    public bool ChaseThrowable()
    {
        return chasingThrowable;
    }

    void SteerTowardsTarget(GameObject target)
    {
        if (steeringRig.IsSeeking)
        {
            if(!(steeringRig.Destination == target.transform.position))
            {
                steeringRig.Destination = target.transform.position;
            }
        }
    }

    //Animation handlers 
    #region Animations (Need more polishing) 
    void Walk(float speed = 1.0f)
    {
        anim.SetFloat(SpeedMultiplier,  walkAnimationSpeed);
        anim.SetBool(IsWalking, true);
        anim.SetBool(IsChasing, false);
        //anim.SetBool("isTaunting", false);
        anim.SetBool(IsAttacking, false);
    }
    
    void Chase(float speed = 1.0f)
    {
        anim.SetFloat(SpeedMultiplier, speed * chaseAnimationSpeed);
        anim.SetBool(IsChasing, true);
        anim.SetBool(IsWalking, false);
        //anim.SetBool("isTaunting", false);
        anim.SetBool(IsAttacking, false);
    }
    #endregion

    IEnumerator Attacking()
    {
        if (_attacking) yield return null; // locks out of other attempt to start the coroutine
        _attacking = true; 
        float timer = 3f;
        while (timer > 0f)
        {
            anim.SetBool(IsWalking, false);
            anim.SetBool(IsChasing, false);
            //anim.SetBool("isTaunting", false);
            anim.SetBool(IsAttacking, true);

            rigidBody.mass = 1000f;
            rigidBody.angularDrag = 1000f;

            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool(IsWalking, true);
        anim.SetBool(IsAttacking, false);
        anim.SetBool(IsChasing, false);
        rigidBody.mass = 10f;
        rigidBody.angularDrag = 35f;

        golemCollider.SetCollisionStatus(false);
        _attacking = false;

        yield return null;
    }
}
