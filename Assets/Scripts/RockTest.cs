using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RockTest : MonoBehaviour
{
    public List<Golem> golems;

    public bool rockStatus;

    public AudioClip rockSfx;
    private AudioSource _sfx;
    private void Start()
    {
        var golemObjects = GameObject.FindGameObjectsWithTag("Enemy");

        golems = new List<Golem>();
        
        foreach(GameObject golem in golemObjects)
        {
            if(golem.GetComponent<Golem>() != null)
            {
                golems.Add(golem.GetComponent<Golem>());

            }
        }

        _sfx = GetComponent<AudioSource>();
        _sfx.spatialBlend = 1.0f;
        _sfx.volume = 0.25f;
        _sfx.maxDistance = 25;
        _sfx.playOnAwake = false;
        _sfx.clip = rockSfx;
        _sfx.loop = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            foreach (Golem golem in golems)
            {
                if((golem.transform.position - transform.position).magnitude > golem.rangeSensor.SensorRange)
                {
                    golem.rangeSensor.IgnoreList.Add(this.gameObject);
                }
            }

            rockStatus = true; 
        }
        if(collision.gameObject.CompareTag("Player")) return;
        _sfx.Play();
    }

    public bool getRockStatus()
    {
        return rockStatus;   
    }
}
