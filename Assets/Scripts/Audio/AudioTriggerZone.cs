using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerZone : MonoBehaviour
{
    [SerializeField] private AudioArea area;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!canChange)
        {
            changeCooldownTimer += Time.deltaTime;
            if (changeCooldownTimer >= changeCooldown)
            {
                changeCooldownTimer = 0f;
                canChange = true;
            }
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (area != null)
            {
                //canChange = false;
                if (!area.activeArea) audioManager.ActivateArea(area);
            }
        }
    }
}
