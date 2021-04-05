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

    //private GuardAI guard;

    private bool attacking;

    void Start()
    {
        anim.GetComponent<Animator>();
    }

    void Update()
    {
        FOVSensor();

        RangeSensor();
    }

    void FOVSensor()
    {
        var detected = fovSensor.GetNearest();

        if (detected != null)
        {
            if (!attacking)
            {
                Chase(detected);

                steeringRig.DestinationTransform = null;
            }
        }
        else
        {
            Walk();
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

                this.gameObject.GetComponent<GuardAI>().enabled = false;

                steeringRig.DestinationTransform = null;
            }
        }
        else
        {
            Walk();
        }
    }

    void Chase(GameObject target)
    {
        transform.LookAt(target.transform);

        transform.position += transform.forward * 1.25f * Time.deltaTime;

        SteerTowardsTarget(target);

        Walk();
    }

    void SteerTowardsTarget(GameObject target)
    {
        if (steeringRig.IsSeeking)
        {
            if(!(steeringRig.Destination == target.transform.position))
            {
                //steeringRig.ClearDirectionToFace();

                steeringRig.Destination = target.transform.position;

            }
        }


    }
    // WIP
    // Used to remove focus from the steering rig so that it may rotate & face towards the target and not be overriden
    // Could be done like this or with a for loop
    //Transform[] RefreshPatrolPath(Transform point)
    //{
    //    Transform[] refreshedPath = new Transform[guard.PatrolPath.Length+1];

    //    guard.PatrolPath.CopyTo(refreshedPath, 0);

    //    refreshedPath[guard.PatrolPath.Length] = point;

    //    return refreshedPath;
    //}

    //Animation handlers 
    #region Animations (Need more polishing) 
    void Walk()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isTaunting", false);
        anim.SetBool("isAttacking", false);
    }

    void Attack()
    {
        attacking = true;

        anim.SetBool("isWalking", false);
        anim.SetBool("isTaunting", false);
        anim.SetBool("isAttacking", true);
    }
    #endregion

    //Golem box collider determines if he's going to attack or not
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Throwable")
        {
            //Destroy throwable?
            rangeSensor.IgnoreList.Add(collision.gameObject);
            steeringRig.IgnoreList.Add(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            Attack();

            rigidBody.mass = 1000f;
            rigidBody.angularDrag = 1000f; 
        }
        else
        {
            rigidBody.mass = 10f;
            rigidBody.angularDrag = 35f;

            attacking = false;
        }       
    }



    //Might add taunt later, starts at the beginning of attack
    //
    //IEnumerator TauntAnimation(float duration)
    //{
    //    float journey = 0f;
    //    while (journey <= duration)
    //    {
    //        journey = journey + Time.deltaTime;

    //        rb.mass = 1000f;

    //        anim.SetBool("isTaunting", true);
    //        anim.SetBool("isWalking", false);
    //    }

    //    yield return null;
    //}
}
