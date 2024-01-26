using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBurnableObject : MonoBehaviour
{

    [SerializeField] private float sphereRadius = 0.27f;

    void Start()
    {
        CheckForIBurnableObjects();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, sphereRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForIBurnableObjects();
        //Debug.Log(other.gameObject.name);
    }


    private void CheckForIBurnableObjects()
    {

        Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, sphereRadius);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IBurnable burnable))
            {
                burnable.BurnObject();
            } 
        }
    }

}
