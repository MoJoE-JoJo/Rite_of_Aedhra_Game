using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerZone : MonoBehaviour
{
    [SerializeField] private AudioArea area1;
    [SerializeField] private AudioArea area2;

    private bool canChange = true;
    private float changeCooldown = 2f;
    private float changeCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canChange)
        {
            changeCooldownTimer += Time.deltaTime;
            if (changeCooldownTimer >= changeCooldown)
            {
                changeCooldownTimer = 0f;
                canChange = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canChange && area1 != null && area2 != null)
            {
                canChange = false;
                if (area1.activeArea) ActivateArea(area2, area1); //if area1 is activate, deactivate it and activate area2
                else if (area2.activeArea) ActivateArea(area1, area2); //if area2 is activate, deactivate it and activate area1
            }
        }
    }

    private void ActivateArea(AudioArea activate, AudioArea deactivate)
    {
        activate.activeArea = true;
        deactivate.activeArea = false;
        deactivate.StopAudio(true);
        activate.PlayAudio(true);
    }
}
