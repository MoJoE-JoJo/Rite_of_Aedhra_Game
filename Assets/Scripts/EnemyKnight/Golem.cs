using SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{

    public Animator anim;
    public Sensor sensor;
    public Rigidbody rb;

    private bool attacking;

    void Start()
    {
        anim.GetComponent<Animator>();
    }

    void Update()
    {
        //Detection using sensors
        var detected = sensor.GetNearest();
        if (detected != null)
        {
            if (!attacking)
            {
                Chase(detected);
            }  
        }
        else
        {
            Walk();
        }
    }

    void Chase(GameObject player)
    {
        transform.LookAt(player.transform);
        transform.position += transform.forward * 1f * Time.deltaTime;

        Walk();
    }

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
        if (collision.gameObject.tag == "Player")
        {
            Attack();

            rb.mass = 1000f;
            rb.angularDrag = 1000f; 
        }
        else
        {
            rb.mass = 10f;
            rb.angularDrag = 35f;
            attacking = false;
        }       
    }

    //Might add taunt later, starts at beginning of attack
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
