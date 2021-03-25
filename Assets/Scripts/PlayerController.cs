using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour

{

    [SerializeField] private PlayerWasdMove wasdMoveScript;
    [SerializeField] private PlayerClickMove clickMoveScript;
    // Start is called before the first frame update
    private void Start()
    {
        Assert.IsTrue(clickMoveScript != null);
        Assert.IsTrue(wasdMoveScript != null);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
