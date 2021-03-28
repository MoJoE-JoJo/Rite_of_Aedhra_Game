using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour

{

    [SerializeField] private TextMeshProUGUI movementSchemeText;
    private PlayerClickMove _clickMoveScript;
    private PlayerWasdMove _wasdMoveScript;
    // Start is called before the first frame update
    private void Start()
    {
        _clickMoveScript = GetComponent<PlayerClickMove>();
        _wasdMoveScript = GetComponent<PlayerWasdMove>();
        Assert.IsTrue(_clickMoveScript != null);
        Assert.IsTrue(_wasdMoveScript != null);
        movementSchemeText.text = (_clickMoveScript.enabled) ? "Click to move" : "WASD to move";
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clickMoveScript.enabled = !_clickMoveScript.enabled;
            _wasdMoveScript.enabled = !_wasdMoveScript.enabled;
            movementSchemeText.text = (_clickMoveScript.enabled) ? "Click to move" : "WASD to move";
        }
        Assert.IsFalse(_clickMoveScript.enabled && _wasdMoveScript.enabled);
    }
}
