using Game_Systems;
using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
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

    [Range(0.0f, 2.0f)]
    [SerializeField] private float chaseAnimationSpeed = 1.0f;

    public float maxSpeed = 3.5f;
    public float acceleration = 0.15f;
    public float speed = 1.0f;
    private bool _attacking;
    private bool _firstDetect = true;

    private static readonly int SpeedMultiplier = Animator.StringToHash("speedMultiplier");
    private static readonly int IsCrawling = Animator.StringToHash("isCrawling");

    private HurtBox _hurtBox;

    void Start()
    {
        anim.GetComponent<Animator>();
        anim.Update(Random.value);
        eyes.GetComponent<MeshCollider>();
        _hurtBox = gameObject.GetComponentInChildren<HurtBox>();
    }

    void Update()
    {
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
                    GetComponentInChildren<Shout>().PlayShout();

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
            chasingThrowable = false;
        }
    }

    private void ResetSpeed()
    {
        speed = 1.0f;
    }

    private void ChaseTarget(GameObject target)
    {     
        transform.LookAt(target.transform);

        if (chasingThrowable)
        {
            speed = 0.7f;
        }
        if (chasingPlayer)
        {
            if (speed <= maxSpeed)
            {
                speed += acceleration * Time.deltaTime;

                Chase(speed);
            }

        }

        Transform transformVar = transform;
        transformVar.position += transformVar.forward * (speed * Time.deltaTime);
        SteerTowardsTarget(target);      
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

    void Chase(float speed = 1.0f)
    {
        anim.SetFloat(SpeedMultiplier, speed * chaseAnimationSpeed);
        anim.SetBool(IsCrawling, true);
    }

    #endregion

    IEnumerator Attacking()
    {
        if (_attacking) yield return null; // locks out of other attempt to start the coroutine
        _attacking = true; 
        float timer = 3f;
        while (timer > 0f)
        {
            rigidBody.mass = 1000f;
            rigidBody.angularDrag = 1000f;

            timer -= Time.deltaTime;
            yield return null;
        }
        rigidBody.mass = 10f;
        rigidBody.angularDrag = 35f;

        golemCollider.SetCollisionStatus(false);
        _attacking = false;

        yield return null;
    }
}
