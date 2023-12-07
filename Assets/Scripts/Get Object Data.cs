using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GetObjectData : MonoBehaviour
{

    [SerializeField] 
    private float rayDistance;

    [SerializeField] 
    private LayerMask ignoreLayer;

    public GameObject GetGameObject()
    {
        Vector2 camera = new(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(camera);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, ~ignoreLayer, QueryTriggerInteraction.Collide))
        {
            if (hitInfo.collider != null)
            {
                return hitInfo.collider.gameObject;
            }
        }

        return null;
    }

    public ObjectScannable GetScannableObject()
    {
        if (GetGameObject() != null) // Fires and Checks ray from camera forward
        {
            if (GetGameObject().TryGetComponent(out ObjectScannable objectScannable)) // Trys to get ObjectScannable component from ray
            {
                return objectScannable;
            }
        }

        return null;
    }

    public ObjectMorphable GetMorphableObject()
    {
        if (GetGameObject() != null)
        {
            if (GetGameObject().TryGetComponent(out ObjectMorphable objectMorphable))
            {
                return objectMorphable;
            }
        }

        return null;
    }
}
