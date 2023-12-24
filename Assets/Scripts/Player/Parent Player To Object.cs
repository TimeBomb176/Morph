using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayerToObject : MonoBehaviour
{

    private void Update()
    {

        float rayDistance = .5f;

        bool isColliding = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, rayDistance);

        if (isColliding)
        {
            if (hitInfo.transform.CompareTag("Parentable"))
            {
                transform.parent = hitInfo.transform;
            } else NullParent();
        } else NullParent();

        Debug.Log(transform.parent);

    }

    private void NullParent()
    {
        transform.parent = null;
    }

}
