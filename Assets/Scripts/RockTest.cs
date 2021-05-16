using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RockTest : MonoBehaviour
{
    public List<Golem> golems;
    public List<Crawler> crawlers;

    public bool rockStatus;

    public AudioClip rockSfx;
    private AudioSource _sfx;
    private void Start()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        golems = new List<Golem>();
        crawlers = new List<Crawler>();
        
        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<Golem>() != null && enemy.GetComponent<Golem>().enabled)
            {
                golems.Add(enemy.GetComponent<Golem>());
            }
            else if(enemy.GetComponent<Crawler>() != null && enemy.GetComponent<Crawler>().enabled)
            {
                crawlers.Add(enemy.GetComponent<Crawler>());
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (Golem golem in golems)
            {
                if((golem.transform.position - transform.position).magnitude > golem.rangeSensor.SensorRange)
                {
                    golem.rangeSensor.IgnoreList.Add(this.gameObject);
                }
            }
            foreach (Crawler crawler in crawlers)
            {
                if ((crawler.transform.position - transform.position).magnitude > crawler.rangeSensor.SensorRange)
                {
                    crawler.rangeSensor.IgnoreList.Add(this.gameObject);
                }
            }

            rockStatus = true; 
        }
        if(collision.gameObject.CompareTag("Player")) return;
        _sfx.Play();
    }

    public bool GetRockStatus()
    {
        return rockStatus;   
    }
}
