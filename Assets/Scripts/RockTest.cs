using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTest : MonoBehaviour
{
    public List<Golem> golems;

    public bool rockStatus;

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
    }

    public bool getRockStatus()
    {
        return rockStatus;   
    }
}
