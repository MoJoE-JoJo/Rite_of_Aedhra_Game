using SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EK_Prototype : MonoBehaviour
{
    //Only used to change animations, need to be implemented in the future
    public float speed = 2f;
    public Sensor sensor;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var detected = sensor.GetNearest();
        if(detected != null)
        {
            chase(detected);
        }
        else
        {
            walk();
        }
    }

    //Could be built upon, taken from the sensor unity asset
    void chase(GameObject player)
    {
        transform.LookAt(player.transform);
        transform.position += transform.forward * 1f * Time.deltaTime;

        //Cheap way to transition between animations, needs to be changed
        anim.SetBool("isWalking", false);
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", true);
        anim.SetFloat("speed", 2f);
    }

    //Cheap way to transition between animations, needs to be changed
    void idle()
    {
        anim.SetFloat("speed", 0f);
        anim.SetBool("isIdle", true);
        anim.SetBool("isRunning", false);
    }

    //Cheap way to transition between animations, needs to be changed
    void walk()
    {
        anim.SetFloat("speed", 0.5f);
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", false);
    }
}
