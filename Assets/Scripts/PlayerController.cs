using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour

{
    private PlayerClickMove _clickMoveScript;

    // items
    public GameObject rockPrefab;
    [SerializeField]
    private GameObject rockCooldown;

    private int rockCounter = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // get rock prefab
        //rockPrefab = (GameObject)Resources.Load("Prefabs/SM_Env_Rock_014.prefab", typeof(GameObject)); // Can't figure out why, but this aint working.

        // The below loop should be removed, using it temporarily to get the prefab until the above Resources.Load is fixed.
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as UnityEngine.Object[])
        {
            if (go.name == "SM_Env_Rock_014")
            {
                rockPrefab = go;
            }
        }

        _clickMoveScript = GetComponent<PlayerClickMove>();
        Assert.IsTrue(_clickMoveScript != null);
    }

    // Update is called once per frame
    private void Update()
    {
        // Throw rock
        if (Input.GetKeyDown("r"))
        {
            ThrowRock();
        }
    }

    private void ThrowRock()
    {
        // check if rockcooldown is added
        if (!rockCooldown)
        {
            return;
        }
        // start cooldown, if false it is not ready yet.
        if (!rockCooldown.GetComponent<RockCooldown>().StartCooldown())
        {
            return;
        }
        // stop current movement?
        // play throw animation
        // create the rock
        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 origin = transform.position + offset;
        GameObject go = Instantiate(rockPrefab, origin, Quaternion.identity);
        go.name = go.name + rockCounter++;
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        // find the rock throws end location
        // should improve this by ignoring certain object types (trees etc.)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        Vector3 target = hit.point;

        // from https://forum.unity.com/threads/how-to-calculate-force-needed-to-jump-towards-target-point.372288/
        float initialAngle = 40f;
        Rigidbody rigid = go.AddComponent<Rigidbody>();
        rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Vector3 p = target;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(origin.x, 0, origin.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = origin.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        Vector3 direction = (planarTarget - planarPostion);
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, direction);
        if (direction.x < 0)
        {
            angleBetweenObjects = 360 - angleBetweenObjects;
        }
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        rigid.velocity = finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);

    }
}
