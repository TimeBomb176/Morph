using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BurnableObject : MonoBehaviour, IBurnable
{

    public event EventHandler OnObjectBurned;

    public void BurnObject()
    {
        OnObjectBurned?.Invoke(this, EventArgs.Empty);
    }
}
