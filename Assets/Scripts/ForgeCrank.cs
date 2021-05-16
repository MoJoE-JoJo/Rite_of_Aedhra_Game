using Cinemachine;
using DG.Tweening;
using Game_Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeCrank : Item
{
    // Start is called before the first frame update
    private bool activate = false;

    public bool isMoving = false;

    [SerializeField]
    private int onPosition = 1;
    [SerializeField]
    private int currentPosition = 0;
    private float rotationAngle;
    private float startingRotation;
    private float animationTime = 5f;
    private float time = 0;
    private CinemachineBrain _brain;

    [SerializeField] private CinemachineVirtualCamera normalCamera;
    [SerializeField] private CinemachineVirtualCamera shakingCamera;
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject endBoss;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject endFenlin;
    [SerializeField] private GameObject fenlin;
    [SerializeField] private GameObject endTransition;

    void Awake()
    {
        bool buttonOn = GameManager.Instance.GetPuzzleStatus("button");
        bool leverOn = GameManager.Instance.GetPuzzleStatus("lever");

        if (buttonOn && leverOn) activate = true;

        rotationAngle = 180f;
        startingRotation = arm.transform.eulerAngles.y;
        _brain = FindObjectOfType<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!activate || !isMoving)
        {
            return;
        }
        time += Time.deltaTime;
        if (time >= animationTime)
        {
            shakingCamera.Priority = _brain.ActiveVirtualCamera.Priority - 2;
            normalCamera.Priority = _brain.ActiveVirtualCamera.Priority + 1;
            isMoving = false;
            //currentPosition = currentPosition + 1 % positions;
            //isOn = onPosition == currentPosition;
            //OnLockChanged();
            endBoss.SetActive(true);
            endFenlin.SetActive(true);
            fenlin.SetActive(false);
            boss.SetActive(false);
            endTransition.SetActive(true);
            return;
        }
        // animate the lever handle
        Quaternion from = Quaternion.Euler(arm.transform.eulerAngles.x, startingRotation + currentPosition * rotationAngle, arm.transform.eulerAngles.z);
        Quaternion to = Quaternion.Euler(arm.transform.eulerAngles.x, startingRotation + (currentPosition + 1) * rotationAngle, arm.transform.eulerAngles.z);
        arm.transform.rotation = Quaternion.Lerp(from, to, time/animationTime);
        
    }

    public void Rotate()
    {
        // check if pillar can rotate
        if (isMoving)
        {
            return;
        }

        // Start rotating the pillar. The on isn't toggled until the movement is finished.
        time = 0;
        isMoving = true;

        shakingCamera.Priority = _brain.ActiveVirtualCamera.Priority + 1;
        normalCamera.Priority = _brain.ActiveVirtualCamera.Priority - 2;

        //Do more stuff
    }

    override public void InteractWithItem()
    {
        if(activate) Rotate();
    }
}
