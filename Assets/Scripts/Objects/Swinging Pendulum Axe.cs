using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPendulumAxe : MonoBehaviour
{
    [SerializeField] private BurnableObject burnableObject;

    private void Start()
    {
        burnableObject.OnObjectBurned += BurnableObject_OnObjectBurned;
    }

    private void BurnableObject_OnObjectBurned(object sender, System.EventArgs e)
    {
        Debug.Log("Axe Dropped");
    }
}
