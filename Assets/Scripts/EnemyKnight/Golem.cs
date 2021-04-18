using SensorToolkit;
using SensorToolkit.Example;
using System.Collections;
using System.Collections.Generic;
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

    private bool attacking;

    void Start()
    {
        anim.GetComponent<Animator>();
        eyes.GetComponent<MeshCollider>();
    }

    void Update()
    {
        FOVSensor();

        RangeSensor();

        if (golemCollider.CollisionStatus())
        {
            StartCoroutine(Attacking());
        }
        else
        {
            Walk();

            StopAllCoroutines();
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            golemCollider.setCollisionStatus(false);
        }
        
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
            if (!attacking)
            {
                chasingPlayer = true;

                Chase(detected);

                //steeringRig.DestinationTransform = null;

                steeringRig.DestinationTransform = detected.gameObject.transform;
            }
            else
            {
                golemCollider.setCollisionStatus(false);
            }
        }
        else
        {
            Walk();

            chasingPlayer = false;          
        }
    }

    void RangeSensor()
    {
        var detected = rangeSensor.GetNearest();
        var rock = detected?.GetComponent<RockTest>();

        if (detected != null && rock != null && rock.getRockStatus())
        {
            if (!attacking)
            {              
                Chase(detected);

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

    void Chase(GameObject target)
    {
        if (chasingThrowable)
        {
            transform.LookAt(target.transform);

            transform.position += transform.forward * 1.25f * Time.deltaTime;

            SteerTowardsTarget(target);

            Walk();
        }
        if (chasingPlayer)
        {
            transform.LookAt(target.transform);

            transform.position += transform.forward * 2.25f * Time.deltaTime;

            SteerTowardsTarget(target);

            Walk();
        }

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
    void Walk()
    {
        anim.SetBool("isWalking", true);
        //anim.SetBool("isTaunting", false);
        anim.SetBool("isAttacking", false);
    }
    #endregion

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject.tag == "Throwable")
    //    {
    //        //Destroy throwable?
    //        Destroy(collision.gameObject);
    //        //steeringRig.IgnoreList.Add(collision.gameObject);
    //        //chasingThrowable = false;   
    //    }        
    //}

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        //Attack();
    //    }
    //    else
    //    {
    //        attacking = false;
    //    }
    //}

    IEnumerator Attacking()
    {
        float timer = 3f;
        while (timer > 0f)
        {
            anim.SetBool("isWalking", false);
            //anim.SetBool("isTaunting", false);
            anim.SetBool("isAttacking", true);

            rigidBody.mass = 1000f;
            rigidBody.angularDrag = 1000f;

            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
        rigidBody.mass = 10f;
        rigidBody.angularDrag = 35f;

        yield return null;
    }

    //void Attack()
    //{
    //    if (attacking)
    //    {
    //        anim.SetBool("isWalking", false);
    //        //anim.SetBool("isTaunting", false);
    //        anim.SetBool("isAttacking", true);
    //        rigidBody.mass = 1000f;
    //        rigidBody.angularDrag = 1000f;
    //    }
    //    else
    //    {
    //        anim.SetBool("isWalking", true);
    //        //anim.SetBool("isTaunting", false);
    //        anim.SetBool("isAttacking", false);
    //        rigidBody.mass = 10f;
    //        rigidBody.angularDrag = 35f;
    //    }
        
    //}
}
