using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : Item
{
    protected bool isOn = false;
    public bool GetIsOn()
    {
        return isOn;
    }

    public delegate void LockChanged();

    public static event LockChanged LockChangedEvent;

    protected virtual void OnLockChanged()
    {
        LockChangedEvent();
    }
}
