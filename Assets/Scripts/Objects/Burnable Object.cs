using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour, IBurnable
{
    public void BurnObject()
    {
        Debug.Log(this.gameObject.name + " Burned");
    }
}
