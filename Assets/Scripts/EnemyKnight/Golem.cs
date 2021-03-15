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

    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var detected = sensor.GetNearest();
        if (detected != null)
        {
            if (!attacking)
            {
                Chase(detected);
            }
            //TauntAnimation(5000f);

            //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            //{

            //    
            //}
        }
        else
        {
            Walk();
        }
    }


    void Chase(GameObject player)
    {
 
        //rb.mass = 10f;

        transform.LookAt(player.transform);
        transform.position += transform.forward * 1f * Time.deltaTime;

        Walk();
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Attack();

            rb.mass = 1000f;
        }
        else
        {
            rb.mass = 10f;

            attacking = false;
        }       
    }

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
