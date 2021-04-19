using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool AllowInput { get; private set; } = true;

    public static void DisableInput()
    {
        AllowInput = false;
    }

    public static void EnableInput()
    {
        AllowInput = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
