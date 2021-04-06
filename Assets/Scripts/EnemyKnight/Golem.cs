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

        if (GolemCollision.CollisionStatus())
        {
            StartCoroutine(Attacking());
        }
        else
        {
            Walk();
        }
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

                eyes.enabled = true;
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

        if (detected != null && RockTest.getRockStatus())
        {
            if (!attacking)
            {              
                Chase(detected);

                steeringRig.DestinationTransform = detected.gameObject.transform;

                //steeringRig.DestinationTransform = null;

                chasingThrowable = true;

                eyes.enabled = false;
            }
        }
        else
        {
            Walk();

            chasingThrowable = false;

            eyes.enabled = true;
        }
    }

    void Chase(GameObject target)
    {
        transform.LookAt(target.transform);

        transform.position += transform.forward * 1.25f * Time.deltaTime;

        SteerTowardsTarget(target);

        Walk();
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Throwable")
        {
            //Destroy throwable?
            Destroy(collision.gameObject);
            //steeringRig.IgnoreList.Add(collision.gameObject);
            chasingThrowable = false;   
        }

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Attacking());
        }
    }

    IEnumerator Attacking()
    {
        float timer = 1f;
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
}
